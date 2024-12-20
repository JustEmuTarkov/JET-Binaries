﻿using JET.Utilities.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EFT.Hideout;
using UnityEngine;
using UnityEngine.UI;

#if B13074 || B13487 || B14687 || B16029


#if B14687
using Requirement = GClass1549; // EFT.Hideout.RelatedRequirements as Data field (list)
using HideoutInstance = GClass1525; // search for AreaDatas (Member)
#else
#if B16029
using Requirement = GClass1599; // EFT.Hideout.RelatedRequirements as Data field (list)
using HideoutInstance = GClass1575; // search for AreaDatas (Member)
#else
using Requirement = GClass1329; // EFT.Hideout.RelatedRequirements as Data field (list)
using HideoutInstance = GClass1305; // search for AreaDatas (Member)
#endif
#endif



namespace JET.Patches.Other
{
    /*
     Patch created by: VIPkiller17#4326
     Github: https://github.com/TommySoucy/HideoutRequirementIndicator >> https://github.com/TommySoucy/MoreCheckmarks
     */
    class HideoutRequirementIndicator : GenericPatch<HideoutRequirementIndicator>
    {
        public HideoutRequirementIndicator() : base(postfix: nameof(PatchPostfix)) { }
        protected override MethodBase GetTargetMethod()
        {
            var typeOf = typeof(EFT.UI.DragAndDrop.QuestItemViewPanel);
            return typeOf.GetMethod("Show", BindingFlags.Public | BindingFlags.Instance);
        }

        public static void PatchPostfix(
           // EFT.Profile profile, 
            EFT.InventoryLogic.Item item, 
            EFT.UI.SimpleTooltip tooltip, 
            EFT.UI.DragAndDrop.QuestItemViewPanel __instance,
            ref Image ____questIconImage, 
            ref Sprite ____foundInRaidSprite, 
            ref string ___string_3, 
            ref EFT.UI.SimpleTooltip ___simpleTooltip_0
        ) {
            var foundNeeded = false;
            var foundFullfilled = false;
            var areaNames = new List<string>();

            var hideoutInstance = Comfort.Common.Singleton<HideoutInstance>.Instance;
            foreach (var ad in hideoutInstance.AreaDatas)
            {
                var actualNextStage = ad.NextStage;

                // TODO: the following should depend on config
                // If we don't want to get requirement of locked to construct areas, skip if it is locked to construct
                //if(/* !config.showLockedModules &&*/ ad.Status == EFT.Hideout.EAreaStatus.LockedToConstruct)
                //{
                //    continue;
                //}

                switch (ad.Status)
                {
                    // If the area has no future upgrade, skip
                    case EFT.Hideout.EAreaStatus.NoFutureUpgrades:
                        continue;
                    // If in process of constructing or upgrading, go to actual next stage if it exists
                    case EFT.Hideout.EAreaStatus.Constructing:
                    case EFT.Hideout.EAreaStatus.Upgrading:
                    {
                        actualNextStage = ad.StageAt(ad.NextStage.Level + 1);

                        // If there are not StageAt given level, it will return a new stage, so level will be 0
                        if (actualNextStage.Level == 0)
                        {
                            continue;
                        }

                        break;
                    }
                }

                var requirements = actualNextStage.Requirements;

                foreach (var requirement in requirements)
                {
                    if (!(requirement is ItemRequirement itemRequirement)) continue;
                    var requirementTemplate = itemRequirement.TemplateId;
                    if (item.TemplateId != requirementTemplate) continue;
                    // A requirement but already have the amount we need
                    if (requirement.Fulfilled)
                    {
                        // Even if we have enough of this item to fulfill a requirement in one area
                        // we might still need it, and if thats the case we want to show that color, not fulfilled color, so you know you still need more of it
                        // So only set color to fulfilled if not needed
                        if (!foundNeeded && !foundFullfilled)
                        {
                            // Following calls base class method ShowGameObject()
                            // To call base methods without reverse patch, must modify IL code for this line from callvirt to call
                            (__instance as EFT.UI.UIElement).HideGameObject();
                            ____questIconImage.sprite = ____foundInRaidSprite;
                            ____questIconImage.color = new Color(0.23137f, 0.93725f, 1);

                            foundFullfilled = true;
                        }

                        areaNames.Add("<color=#3bdfff>" + ad.Template.Name + "</color>");
                    }
                    else
                    {
                        if (!foundNeeded)
                        {
                            (__instance as EFT.UI.UIElement).HideGameObject();
                            ____questIconImage.sprite = ____foundInRaidSprite;
                            ____questIconImage.color = new Color(0.23922f, 1, 0.44314f);

                            foundNeeded = true;
                        }

                        areaNames.Add("<color=#3dff71>" + ad.Template.Name + "</color>");
                    }
                }
            }

            if (foundNeeded || foundFullfilled)
            {
                // Build string of list of areas this is needed for
                var areaNamesString = "";
                for (var i = 0; i < areaNames.Count; ++i)
                {
                    areaNamesString += (i == 0 ? "" : (areaNames.Count == 2 ? "" : ",") + (i == areaNames.Count - 1 ? " and " : " ")) + areaNames[i];
                }

                if (___string_3 != null && (item.MarkedAsSpawnedInSession || item.QuestItem))
                {
                    ___string_3 += string.Format(" and needed for {0}".Localized(), areaNamesString);
                }
                else
                {
                    ___string_3 = string.Format("Needed for {0}".Localized(), areaNamesString);
                }

                // If this is not a quest item or found in raid, the original returns and the tooltip never gets set, so we need to set it ourselves
                ___simpleTooltip_0 = tooltip;
            }
            else
            {
                // Just to make sure the change is not permanent, because the color is never set back to the default white by EFT
                // Because if an item was a requirement, its sprite's color set to green/blue, then it stopped being a requirement, but it was found in raid/is quest item
                // the sprite would still show up green/blue
                ____questIconImage.color = Color.white;
            }
        }
    }
}
#endif
