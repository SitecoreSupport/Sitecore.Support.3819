using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.ItemResolvers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.XA.Feature.CreativeExchange.Pipeline.ExportAsset;
using Sitecore.XA.Feature.CreativeExchange.Services.Messages;

namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipeline.ExportAsset
{
    using System;
    using System.Collections.Generic;

    public class StoreMediaItem : IExportAsset
    {
        private static readonly ItemPathResolver resolver = new MixedItemNameResolver(new ItemPathResolver());
        public void Process(ExportAssetArgs args)
        {

            if (!args.IsThemeAsset && args.AssetModel.PageUrl.LastIndexOf(".") > 0)
            {
                try
                {
                    // Context.ContentDatabase.GetItem(ID.Parse(this._pageId))
                    Item root = Context.ContentDatabase.GetItem("/sitecore/media library");
                    // string itemPath = args.AssetModel.PageUrl.Replace("-/media", "/sitecore/media library");
                    string itemPath = args.AssetModel.PageUrl.Replace("-/media", "");
                    int fileExtPos = itemPath.LastIndexOf(".");
                    if (fileExtPos >= 0)
                        itemPath = itemPath.Substring(0, fileExtPos);
                    Item resolvedItem = resolver.ResolveItem(itemPath, root);
                    if (resolvedItem != null)
                    {
                        if (resolvedItem.Paths.IsMediaItem)
                        {
                            List<MessageBase> collection = args.Config.ExchangeStore.AddEntry(args.AssetModel.PageUrl, resolvedItem);
                            args.Messages.AddRange(collection);
                        }
                    }

                }
                catch (Exception ex)
                {

                    Log.Error(ex.Message, this);
                }


            }
        }
    }
}
