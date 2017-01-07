using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Settings;
using Orchard.Core.Contents.ViewModels;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Messaging.Services;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Users.Models;
using Orchard.ContentManagement.Records;

namespace Pfr.Cases.Controllers {
    [ValidateInput(false), Themed]
    public class CasesController : Controller, IUpdateModel {

        private readonly IMembershipService _membershipService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ISiteService _siteService;
        private readonly IShapeDisplay _shapeDisplay;
        private readonly IMessageService _messageService;

        public CasesController(IOrchardServices services,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            ITransactionManager transactionManager,
            ISiteService siteService,
            IShapeFactory shapeFactory,
            IShapeDisplay shapeDisplay,
            IMembershipService membershipService,
            IMessageService messageService) {

            _membershipService = membershipService;

            Services = services;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _transactionManager = transactionManager;
            _siteService = siteService;

            _messageService = messageService;
            Shape = shapeFactory;
            _shapeDisplay = shapeDisplay;
        }

        private IOrchardServices Services { get; set; }
        dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index(int id) {
            var casePart = _contentManager.Get<ContentPart>(id, VersionOptions.Published);
            dynamic shape = Services.ContentManager.BuildDisplay(casePart.ContentItem);
            return View((object)shape);
            //var caseContentItems = _contentManager.Query().ForType("Case").List();
            //foreach (var item in caseContentItems)
            //{
            //    var contentItem = (ContentItem)item.Content;
            //    ContentItemVersionRecord contentItemRecord = contentItem.VersionRecord;
            //    string data = contentItemRecord.Data;
            //    //Call some function here to parse 'data' and store the object in the list. 
            //}
            //var list = Shape.List();
            //list.AddRange(caseContentItems.Select(ci => _contentManager.BuildDisplay(ci, "Index")));
            ////list.Add(BuildDisplay(contentItem, "Index"));
            //var viewModel = Shape.ViewModel()
            //    .ContentItems(list)
            //    //.Pager(pagerShape)
            //    .TypeDisplayName("Case");

            //return View(viewModel);
        }

        public ActionResult SendMail(int id)
        {
            var casePart = _contentManager.Get<ContentPart>(id, VersionOptions.Published);

            var sender = Services.WorkContext.CurrentUser;
            var recipient = Request.Form["txtEmail"];
            if (sender != null && recipient != string.Empty)
            {
                dynamic shape = Services.ContentManager.BuildDisplay(casePart.ContentItem);
                
                var parameters = new Dictionary<string, object> {
                            {"Subject", T("FastReport.org: Case details").Text},
                            {"Body", _shapeDisplay.Display(shape)},
                            {"From", sender.Email },
                            {"Recipients", recipient }
                        };

                _messageService.Send("Email", parameters);

                //return View((object)shape);
            }
            return RedirectToAction("Index", new { id = id });
        }
        public IUser GetUser(string username)
        {
            var lowerName = username == null ? "" : username.ToLowerInvariant();

            return Services.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == lowerName).List().FirstOrDefault();
        }
        private IEnumerable<ContentTypeDefinition> GetListableTypes(bool andContainable)
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd =>
                ctd.Name == "Case" &&
                ctd.Settings.GetModel<ContentTypeSettings>().Listable &&
                (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart")));
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}