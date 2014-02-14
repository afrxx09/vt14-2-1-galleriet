using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageGallery.Model;

namespace ImageGallery
{
    public partial class Default : System.Web.UI.Page
    {
        private Service _service;
        private Service Service { get{ return _service ?? (_service = new Service()); } }

        private string _imageQuery;
        private string ImageQuery { get { return _imageQuery ?? (_imageQuery = Request.QueryString["i"]); } }

        private string _successQuery;
        private string SuccessQuery { get { return _successQuery ?? (_successQuery = Request.QueryString["success"]); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(ImageQuery != null){
                PreviewImage.ImageUrl = "~/Content/Images/" + ImageQuery;
                PreviewImage.Visible = true;
                if (SuccessQuery != null)
                {
                    ShowUploadSuccess(ImageQuery);
                }
            }
        }

        public IEnumerable<System.String> ImageRepeater_GetData()
        {
            return Service.GetCachedImages();
        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    string uploadedFile = Service.UploadImage(fiUploadGalleryImage.PostedFile);
                    Response.Redirect(String.Format("?i={0}&success=true", uploadedFile));
                    
                }
                catch (ArgumentOutOfRangeException ae)
                {
                    phFail.Visible = true;
                    ModelState.AddModelError(String.Empty, ae.Message);
                }
                catch
                {
                    phFail.Visible = true;
                    ModelState.AddModelError(String.Empty, "Något gick fel vid uppladdning");
                }
            }
            else
            {
                phFail.Visible = true;
            }
        }

        private void ShowUploadSuccess(string uploadedFile)
        {
            phSuccess.Visible = true;
            lblUploadSuccess.Text = String.Format("Bilden '{0}' har laddats upp", uploadedFile);
        }
    }
}