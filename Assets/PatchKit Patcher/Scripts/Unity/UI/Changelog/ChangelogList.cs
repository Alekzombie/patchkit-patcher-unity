﻿using System.Collections;
using System.Linq;
using PatchKit.Api.Models.Main;
using PatchKit.Patcher.Utilities;

namespace PatchKit.Patcher.Unity.UI.Changelog
{
    public class ChangelogList : UIApiComponent
    {
        public ChangelogElement TitlePrefab;

        public ChangelogElement ChangePrefab;

        protected override IEnumerator LoadCoroutine()
        {
            while (!UnityPatcher.Instance.Data.HasValue || UnityPatcher.Instance.Data.Value.AppSecret == null)
            {
                yield return null;
            }

            yield return
                UnityThreading.StartThreadCoroutine(() => MainApiConnection.GetAppVersionList(UnityPatcher.Instance.Data.Value.AppSecret),
                    response =>
                    {
                        foreach (var version in response.OrderByDescending(version => version.Id))
                        {
                            CreateVersionChangelog(version);
                        }
                    });
        }

        private void CreateVersionChangelog(AppVersion version)
        {
            CreateVersionTitle(version.Label);
            CreateVersionChangeList(version.Changelog);
        }

        private void CreateVersionTitle(string label)
        {
            var title = Instantiate(TitlePrefab);
            title.Text.text = string.Format("Changelog {0}", label);
            title.transform.SetParent(transform, false);
            title.transform.SetAsLastSibling();
        }

        private void CreateVersionChangeList(string changelog)
        {
            var changeList = (changelog ?? string.Empty).Split('\n');

            foreach (var change in changeList.Where(s => !string.IsNullOrEmpty(s)))
            {
                string formattedChange = change.TrimStart(' ', '-', '*');
                CreateVersionChange(formattedChange);
            }
        }

        private void CreateVersionChange(string changeText)
        {
            var change = Instantiate(ChangePrefab);
            change.Text.text = changeText;
            change.transform.SetParent(transform, false);
            change.transform.SetAsLastSibling();
        }
    }
}