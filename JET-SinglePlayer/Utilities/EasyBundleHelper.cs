﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
#if B16029

/*
        Class2559 for 16029 house these variables that're being matched(?)
        All the variables that class EasyBundleHelper seems to be matching? are in Class2559

        The BindableState reference is similar to Class2485

        IBundleLock - IsLocked property is in GInterface273
*/


using IBundleLock = GInterface275; //Property: IsLocked
    
/*
 * Original:
 * GInterface275; //Property: IsLocked
 * 
public interface GInterface273
{
	// Token: 0x17001ADD RID: 6877
	// (get) Token: 0x0600CADC RID: 51932
	bool IsLocked { get; }

	// Token: 0x0600CADD RID: 51933
	void Lock();

	// Token: 0x0600CADE RID: 51934
	void Unlock();
}
*/

using BindableState = GClass2534<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue

/* 
 * Original:
 * GClass2534<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
 * 
 * Example how bindable state looks like ** from version 14687
public sealed class GClass2464<T> : GClass2463<T>
{
	// Token: 0x17001A96 RID: 6806
	// (set) Token: 0x0600C8D8 RID: 51416 RVA: 0x0011EAA7 File Offset: 0x0011CCA7
	public override T Value
	{
		set
		{
			base.method_0(value);
		}
	}

	// Token: 0x0600C8D9 RID: 51417 RVA: 0x0011EAB0 File Offset: 0x0011CCB0
	public GClass2464() : base(null)
	{
	}

	// Token: 0x0600C8DA RID: 51418 RVA: 0x0011EAB9 File Offset: 0x0011CCB9
	public GClass2464([CanBeNull] T initialValue, IEqualityComparer<T> equalityComparer = null) : base(equalityComparer)
	{
		this.gparam_0 = initialValue;
	}
}
*/

#endif
#if B14687
using IBundleLock = GInterface272; //Property: IsLocked
using BindableState = GClass2464<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue

#endif
#if B13074 || B13487
using IBundleLock = GInterface264; //Property: IsLocked
using BindableState = GClass2251<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
#if B11661 || B12102
using IBundleLock = GInterface254; //Property: IsLocked
using BindableState = GClass2206<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
#if B10988
using IBundleLock = GInterface251; //Property: IsLocked
using BindableState = GClass2166<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
#if B9767
using IBundleLock = GInterface239; //Property: IsLocked
using BindableState = GClass2100<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
#if B9018
using IBundleLock = GInterface224; //Property: IsLocked
using BindableState = GClass2046<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
#if DEBUG
using IBundleLock = GInterface251; //Property: IsLocked
using BindableState = GClass2166<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue
#endif
namespace JET.Utilities
{
    class EasyBundleHelper
    {
        private readonly object _instance;
        private readonly Traverse _trav;

        private static readonly string _pathFieldName = "string_1";
        private static readonly string _keyWithoutExtensionFieldName = "string_0";
        private static readonly string _loadingJobPropertyName = "task_0";
        private static readonly string _dependencyKeysPropertyName = "DependencyKeys";
        private static readonly string _keyPropertyName = "Key";
        private static readonly string _loadStatePropertyName = "LoadState";
        private static readonly string _progressPropertyName = "Progress";
        private static readonly string _bundlePropertyName = "assetBundle_0";
        private static readonly string _loadingAssetOperationFieldName = "assetBundleRequest_0";
        private static readonly string _assetsPropertyName = "Assets";
        private static readonly string _sameNameAssetPropertyName = "SameNameAsset";
        private static MethodInfo _loadingCoroutineMethod;

        public IEnumerable<string> DependencyKeys
        {
            get
            {
                return _trav.Property<IEnumerable<string>>(_dependencyKeysPropertyName).Value;
            }

            set
            {
                _trav.Property<IEnumerable<string>>(_dependencyKeysPropertyName).Value = value;
            }
        }

        public IBundleLock BundleLock
        {
            get
            {
                return _trav.Field<IBundleLock>($"{typeof(IBundleLock).Name.ToLower()}_0").Value;
            }

            set
            {
                _trav.Field<IBundleLock>($"{typeof(IBundleLock).Name.ToLower()}_0").Value = value;
            }
        }

        public Task LoadingJob
        {
            get
            {
                return _trav.Field<Task>(_loadingJobPropertyName).Value;
            }

            set
            {
                _trav.Field<Task>(_loadingJobPropertyName).Value = value;
            }
        }

        public string Path
        {
            get
            {
                return _trav.Field<string>(_pathFieldName).Value;
            }

            set
            {
                _trav.Field<string>(_pathFieldName).Value = value;
            }
        }

        public string Key
        {
            get
            {
                return _trav.Property<string>(_keyPropertyName).Value;
            }

            set
            {
                _trav.Property<string>(_keyPropertyName).Value = value;
            }
        }

        public BindableState LoadState
        {
            get
            {
                return _trav.Property<BindableState>(_loadStatePropertyName).Value;
            }

            set
            {
                _trav.Property<BindableState>(_loadStatePropertyName).Value = value;
            }
        }

        public float Progress
        {
            get
            {
                return _trav.Property<float>(_progressPropertyName).Value;
            }

            set
            {
                _trav.Property<float>(_progressPropertyName).Value = value;
            }
        }

        
        public AssetBundle Bundle
        {
            get
            {
                return _trav.Field<AssetBundle>(_bundlePropertyName).Value;
            }

            set
            {
                _trav.Field<AssetBundle>(_bundlePropertyName).Value = value;
            }
        }
        
        public AssetBundleRequest loadingAssetOperation
        {
            get
            {
                return _trav.Field<AssetBundleRequest>(_loadingAssetOperationFieldName).Value;
            }

            set
            {
                _trav.Field<AssetBundleRequest>(_loadingAssetOperationFieldName).Value = value;
            }
        }


        public Object[] Assets
        {
            get
            {
                return _trav.Property<UnityEngine.Object[]>(_assetsPropertyName).Value;
            }

            set
            {
                _trav.Property<UnityEngine.Object[]>(_assetsPropertyName).Value = value;
            }
        }

        public UnityEngine.Object SameNameAsset
        {
            get
            {
                return _trav.Property<UnityEngine.Object>(_sameNameAssetPropertyName).Value;
            }

            set
            {
                _trav.Property<UnityEngine.Object>(_sameNameAssetPropertyName).Value = value;
            }
        }

        public string KeyWithoutExtension
        {
            get
            {
                return _trav.Field<string>(_keyWithoutExtensionFieldName).Value;
            }

            set
            {
                _trav.Field<string>(_keyWithoutExtensionFieldName).Value = value;
            }
        }

        public EasyBundleHelper(object easyBundle)
        {
            _instance = easyBundle;
            _trav = Traverse.Create(easyBundle);

            if (_loadingCoroutineMethod == null)
            {
                _loadingCoroutineMethod = easyBundle.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Single(x => x.GetParameters().Length == 0 && x.ReturnType == typeof(Task));
                //TODO:Search member names by condition
            }
        }

        public Task LoadingCoroutine()
        {
            return (Task)_loadingCoroutineMethod.Invoke(_instance, new object[] { });
        }
    }
}
