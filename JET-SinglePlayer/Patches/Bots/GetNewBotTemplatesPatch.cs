using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using JET.Utilities.Patching;
using UnityEngine;
#if B15317
//using WaveInfo = GClass1207; // not used but changed anyways in case it does // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass565; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface18; // Method: ChooseProfile
using PoolManager = GClass1458; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2497; // Delegate: Immediate also has General / Low
#endif
#if B14687
//using WaveInfo = GClass984; // not used // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass564; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface18; // Method: ChooseProfile
using PoolManager = GClass1435; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2446; // Delegate: Immediate also has General / Low
#endif
#if B13487
//using WaveInfo = GClass984; // not used // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass379; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface17; // Method: ChooseProfile
using PoolManager = GClass1230; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2232; // Delegate: Immediate also has General / Low
#endif
#if B13074
using WaveInfo = GClass984; // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass379; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface17; // Method: ChooseProfile
using PoolManager = GClass1230; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2232; // Delegate: Immediate also has General / Low
#endif
#if B11661 || B12102
using WaveInfo = GClass956; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass363; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface16; // Method: ChooseProfile
using PoolManager = GClass1198; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2186; // Delegate: Immediate
#endif
#if B10988
using WaveInfo = GClass929; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass362; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface15; // Method: ChooseProfile
using PoolManager = GClass1168; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2146; // Delegate: Immediate
#endif
#if B9767
using WaveInfo = GClass904; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass337; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface14; // Method: ChooseProfile
using PoolManager = GClass1133; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2080; // Delegate: Immediate
#endif
#if B9018
using WaveInfo = GClass897; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass334; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface13; // Method: ChooseProfile
using PoolManager = GClass1120; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2026; // Delegate: Immediate
#endif
#if DEBUG
using WaveInfo = GClass929; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass362; // Method: GetNewProfile (higher GClass number)
using BotData = GInterface15; // Method: ChooseProfile
using PoolManager = GClass1168; // CancellationToken: PoolsCancellationToken
using JobPriority = GClass2146; // Delegate: Immediate
#endif

namespace JET.Patches.Bots
{
    public class GetNewBotTemplatesPatch : GenericPatch<GetNewBotTemplatesPatch>
    {
        public static FieldInfo __field;
        private static Func<BotsPresets, BotData, Profile> _getNewProfileFunc;

        public GetNewBotTemplatesPatch() : base(prefix: nameof(PatchPrefix))
        {
        }

        protected override MethodBase GetTargetMethod()
        {
            foreach (var type in PatcherConstants.TargetAssembly.GetTypes())
            {
                if (type.Name.Contains("GClass"))
                {
                    // its proper gclass now lets check if our targeted method exists there
                    var TargetedMethod = type.GetMethod("method_1", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (TargetedMethod == null)
                    {
                        continue;
                    }
                    MethodBase CreateProfileMethod = null;
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
                        if (method.Name == "CreateProfile" && method.GetParameters().Length == 1 && method.ReturnType.Name == "Task`1")
                            CreateProfileMethod = method;
                    }

                    if (CreateProfileMethod == null)
                    {
                        continue;
                    }
                    if (CreateProfileMethod.GetParameters().Length != 1) 
                        continue;


                    _getNewProfileFunc = type.GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance)
                        .CreateDelegate(typeof(Func<BotsPresets, BotData, Profile>)) as Func<BotsPresets, BotData, Profile>;
                    if (_getNewProfileFunc == null) return null;
                    return CreateProfileMethod;
                }
            }
            return null;
        }

        public static bool PatchPrefix(ref Task<Profile> __result, BotsPresets __instance, BotData data)
        {
            int i = 0;
            /*
                in short when client wants new bot and GetNewProfile() return null (if not more available templates or they don't satisfied by Role and Difficulty condition)
                then client gets new piece of WaveInfo collection (with Limit = 30 by default) and make request to server
                but use only first value in response (this creates a lot of garbage and cause freezes)
                after patch we request only 1 template from server

                along with other patches this one causes to call data.PrepareToLoadBackend(1) gets the result with required role and difficulty:
                new[] { new WaveInfo() { Limit = 1, Role = role, Difficulty = difficulty } }
                then perform request to server and get only first value of resulting single element collection
            */
            var session = Utilities.Config.BackEndSession;
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var taskAwaiter = (Task<Profile>)null;

            // try get profile from cache
            var profile = _getNewProfileFunc(__instance, data);

            if (profile == null)
            {
                // load from server
                Debug.LogError("[JET]: Loading bot profile from server");
                var source = data.PrepareToLoadBackend(1).ToList();
                taskAwaiter = session.LoadBots(source).ContinueWith(GetFirstResult, taskScheduler);
            }
            else
            {
                // return cached profile
                Debug.LogError("[JET]: Loading bot profile from cache");
                taskAwaiter = Task.FromResult(profile);
            }

            // load bundles for bot profile
            var continuation = new Continuation(taskScheduler);

            __result = taskAwaiter.ContinueWith(continuation.LoadBundles, taskScheduler).Unwrap();

            return false;
        }

        private static Profile GetFirstResult(Task<Profile[]> task)
        {
            return task.Result[0];
        }

        private struct Continuation
        {
            Profile Profile;
            TaskScheduler TaskScheduler { get; }

            public Continuation(TaskScheduler taskScheduler)
            {
                Profile = null;
                TaskScheduler = taskScheduler;
            }

            public Task<Profile> LoadBundles(Task<Profile> task)
            {
                Profile = task.Result;

                var loadTask = Singleton<PoolManager>.Instance
                    .LoadBundlesAndCreatePools(PoolManager.PoolsCategory.Raid, 
                                               PoolManager.AssemblyType.Local, 
                                               Profile.GetAllPrefabPaths(false).ToArray(), 
                                               JobPriority.General, 
                                               null, 
                                               default(CancellationToken));

                return loadTask.ContinueWith(GetProfile, TaskScheduler);
            }

            private Profile GetProfile(Task task)
            {
                return Profile;
            }
        }
    }
}
