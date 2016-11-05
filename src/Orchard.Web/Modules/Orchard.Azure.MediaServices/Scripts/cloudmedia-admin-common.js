/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

/// <reference path="Typings/jquery.d.ts" />
var Orchard;
(function (Orchard) {
    var Azure;
    (function (Azure) {
        var MediaServices;
        (function (MediaServices) {
            var Admin;
            (function (Admin) {
                var Common;
                (function (Common) {
                    $(function () {
                        $("form").on("click", "button[data-prompt], a[data-prompt]", function (e) {
                            var prompt = $(this).data("prompt");
                            if (!confirm(prompt))
                                e.preventDefault();
                        });
                    });
                })(Common = Admin.Common || (Admin.Common = {}));
            })(Admin = MediaServices.Admin || (MediaServices.Admin = {}));
        })(MediaServices = Azure.MediaServices || (Azure.MediaServices = {}));
    })(Azure = Orchard.Azure || (Orchard.Azure = {}));
})(Orchard || (Orchard = {}));

//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImNsb3VkbWVkaWEtYWRtaW4tY29tbW9uLmpzIiwiY2xvdWRtZWRpYS1hZG1pbi1jb21tb24udHMiXSwibmFtZXMiOlsiT3JjaGFyZCIsIk9yY2hhcmQuQXp1cmUiLCJPcmNoYXJkLkF6dXJlLk1lZGlhU2VydmljZXMiLCJPcmNoYXJkLkF6dXJlLk1lZGlhU2VydmljZXMuQWRtaW4iLCJPcmNoYXJkLkF6dXJlLk1lZGlhU2VydmljZXMuQWRtaW4uQ29tbW9uIl0sIm1hcHBpbmdzIjoiQUFBQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsQUNMQSw0Q0FBNEM7QUFFNUMsSUFBTyxPQUFPLENBU2I7QUFURCxXQUFPLE9BQU87SUFBQ0EsSUFBQUEsS0FBS0EsQ0FTbkJBO0lBVGNBLFdBQUFBLEtBQUtBO1FBQUNDLElBQUFBLGFBQWFBLENBU2pDQTtRQVRvQkEsV0FBQUEsYUFBYUE7WUFBQ0MsSUFBQUEsS0FBS0EsQ0FTdkNBO1lBVGtDQSxXQUFBQSxLQUFLQTtnQkFBQ0MsSUFBQUEsTUFBTUEsQ0FTOUNBO2dCQVR3Q0EsV0FBQUEsTUFBTUEsRUFBQ0EsQ0FBQ0E7b0JBQzdDQyxDQUFDQSxDQUFDQTt3QkFDRUEsQ0FBQ0EsQ0FBQ0EsTUFBTUEsQ0FBQ0EsQ0FBQ0EsRUFBRUEsQ0FBQ0EsT0FBT0EsRUFBRUEscUNBQXFDQSxFQUFFQSxVQUFTQSxDQUFDQTs0QkFDbkUsSUFBSSxNQUFNLEdBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQzs0QkFFcEMsRUFBRSxDQUFDLENBQUMsQ0FBQyxPQUFPLENBQUMsTUFBTSxDQUFDLENBQUM7Z0NBQ2pCLENBQUMsQ0FBQyxjQUFjLEVBQUUsQ0FBQzt3QkFDM0IsQ0FBQyxDQUFDQSxDQUFDQTtvQkFDUEEsQ0FBQ0EsQ0FBQ0EsQ0FBQ0E7Z0JBQ1BBLENBQUNBLEVBVHdDRCxNQUFNQSxHQUFOQSxZQUFNQSxLQUFOQSxZQUFNQSxRQVM5Q0E7WUFBREEsQ0FBQ0EsRUFUa0NELEtBQUtBLEdBQUxBLG1CQUFLQSxLQUFMQSxtQkFBS0EsUUFTdkNBO1FBQURBLENBQUNBLEVBVG9CRCxhQUFhQSxHQUFiQSxtQkFBYUEsS0FBYkEsbUJBQWFBLFFBU2pDQTtJQUFEQSxDQUFDQSxFQVRjRCxLQUFLQSxHQUFMQSxhQUFLQSxLQUFMQSxhQUFLQSxRQVNuQkE7QUFBREEsQ0FBQ0EsRUFUTSxPQUFPLEtBQVAsT0FBTyxRQVNiIiwiZmlsZSI6ImNsb3VkbWVkaWEtYWRtaW4tY29tbW9uLmpzIiwic291cmNlc0NvbnRlbnQiOltudWxsLCIvLy8gPHJlZmVyZW5jZSBwYXRoPVwiVHlwaW5ncy9qcXVlcnkuZC50c1wiIC8+XG5cbm1vZHVsZSBPcmNoYXJkLkF6dXJlLk1lZGlhU2VydmljZXMuQWRtaW4uQ29tbW9uIHtcbiAgICAkKCgpID0+IHtcbiAgICAgICAgJChcImZvcm1cIikub24oXCJjbGlja1wiLCBcImJ1dHRvbltkYXRhLXByb21wdF0sIGFbZGF0YS1wcm9tcHRdXCIsIGZ1bmN0aW9uKGUpIHtcbiAgICAgICAgICAgIHZhciBwcm9tcHQgPSAkKHRoaXMpLmRhdGEoXCJwcm9tcHRcIik7XG5cbiAgICAgICAgICAgIGlmICghY29uZmlybShwcm9tcHQpKVxuICAgICAgICAgICAgICAgIGUucHJldmVudERlZmF1bHQoKTtcbiAgICAgICAgfSk7XG4gICAgfSk7XG59ICJdLCJzb3VyY2VSb290IjoiL3NvdXJjZS8ifQ==