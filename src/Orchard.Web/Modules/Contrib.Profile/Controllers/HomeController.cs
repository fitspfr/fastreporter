using System;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace Contrib.Profile.Controllers {
    [ValidateInput(false), Themed]
    public class HomeController : Controller, IUpdateModel {

        private readonly IMembershipService _membershipService;

        public HomeController(IOrchardServices services,
            IMembershipService membershipService) {

            _membershipService = membershipService;

            Services = services;
        }

        private IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index(string username) {
            IUser user = _membershipService.GetUser(username);

            //if(user == null || !Services.Authorizer.Authorize(Permissions.ViewProfiles, user, null)) {
            if (user == null && Services.WorkContext.CurrentUser == null)
            {
                return HttpNotFound();
            }
            if (user == null && Services.WorkContext.CurrentUser != null) // Display current user if username not sent.
                user = Services.WorkContext.CurrentUser;
            if(!Services.Authorizer.Authorize(Permissions.ViewProfiles, user, null)) // Return 404 if not authorized to view profile
            {
                return HttpNotFound();
            }

            dynamic shape = Services.ContentManager.BuildDisplay(user.ContentItem);

            return View((object)shape);
        }

        public ActionResult Edit() {
            if(Services.WorkContext.CurrentUser == null) {
                return HttpNotFound();
            }

            IUser user = Services.WorkContext.CurrentUser;

            //var editor = Shape.EditorTemplate(TemplateName: "Parts/User.Edit", Model: new UserEditViewModel { User = user }, Prefix: null);
            //editor.Metadata.Position = "2";
            //var model = Services.ContentManager.BuildEditor(user);
            //model.Content.Add(editor);

            //return View(model);
            dynamic shape = Services.ContentManager.BuildEditor(user.ContentItem);

            return View((object)shape);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost() {
            if (Services.WorkContext.CurrentUser == null) {
                return HttpNotFound();
            }

            IUser user = Services.WorkContext.CurrentUser;

            dynamic shape = Services.ContentManager.UpdateEditor(user.ContentItem, this);
            if (!ModelState.IsValid) {
                Services.TransactionManager.Cancel();
                return View("Edit", (object)shape);
            }

            Services.Notifier.Information(T("Your profile has been saved."));

            return RedirectToAction("Edit");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}