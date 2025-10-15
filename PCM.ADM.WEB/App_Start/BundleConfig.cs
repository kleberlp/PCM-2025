using System.Web;
using System.Web.Optimization;

namespace PCM.ADM.WEB
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS style (bootstrap/PCM.ADM.WEB)
            bundles.Add(new StyleBundle("~/Content/codebase").Include(
                        "~/Content/css/codebase.css",
                        "~/Content/js/plugins/sweetalert2/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                        "~/Content/js/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css"));

            bundles.Add(new StyleBundle("~/Content/select").Include(
                        "~/Content/js/plugins/select2/select2.min.css",
                        "~/Content/js/plugins/select2/select2-bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/colorpicker").Include(
                        "~/Content/js/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css"));
                      
            bundles.Add(new StyleBundle("~/Content/tagsinput").Include(
                        "~/Content/js/plugins/jquery-tags-input/jquery.tagsinput.min.css"));
                      
            bundles.Add(new StyleBundle("~/Content/autocomplete").Include(
                        "~/Content/js/plugins/jquery-auto-complete/jquery.auto-complete.min.css"));
                      
            bundles.Add(new StyleBundle("~/Content/rangeSlider").Include(
                        "~/Content/js/plugins/ion-rangeslider/css/ion.rangeSlider.min.css"));
                      
            bundles.Add(new StyleBundle("~/Content/rangeSlider").Include(
                        "~/Content/js/plugins/ion-rangeslider/css/ion.rangeSlider.min.css",
                        "~/Content/js/plugins/ion-rangeslider/css/ion.rangeSlider.skinHTML5.min.css"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                        //"~/Content/js/plugins/datatables/dataTables.min.css",
                        "~/Content/js/plugins/datatables/dataTables.bootstrap4.min.css"));

            bundles.Add(new StyleBundle("~/Content/inputfile").Include(
                        "~/Content/js/plugins/inputfile/fileinput.min.css"));

            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                        "~/Content/js/plugins/fullcalendar/fullcalendar.min.css"));
            
            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                        "~/Content/js/plugins/dropzonejs/min/dropzone.min.css"));

            bundles.Add(new StyleBundle("~/Content/imagecropper").Include(
                        "~/Content/js/plugins/cropperjs/cropper.min.css"));

            bundles.Add(new StyleBundle("~/Content/footable").Include(
                        "~/Content/js/plugins/footable/footable.core.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/clockpicker").Include(
                        "~/Content/js/plugins/clockpicker/clockpicker.css", new CssRewriteUrlTransform()));


            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Content/js/plugins/slimscroll/jquery.slimscroll.min.js"));

            // ImageCropper
            bundles.Add(new ScriptBundle("~/plugins/imagecropper").Include(
                        "~/Content/js/plugins/cropperjs/cropper.min.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/plugins/codebase").Include(
                        "~/Content/js/core/jquery.min.js",
                        "~/Content/js/core/bootstrap.bundle.min.js",
                        "~/Content/js/core/jquery.slimscroll.min.js",
                        "~/Content/js/core/jquery.scrollLock.min.js",
                        "~/Content/js/core/jquery.appear.min.js",
                        "~/Content/js/core/jquery.countTo.min.js",
                        "~/Content/js/core/js.cookie.min.js",
                        "~/Content/js/codebase.js",
                        "~/Content/js/plugins/jquery-inputmask/jquery.inputmask.js",
                        "~/Content/js/plugins/sweetalert2/sweetalert2.min.js",
                        "~/Content/js/plugins/dropzonejs/min/dropzone.min.js"));

            //CKEditor
            bundles.Add(new ScriptBundle("~/plugins/ckeditor").Include(
                        "~/Content/js/plugins/ckeditor/ckeditor.js"));

            //DataTables
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                        "~/Content/js/plugins/dataTables/jquery.dataTables.min.js",
                        "~/Content/js/plugins/dataTables/dataTables.min.js",
                        "~/Content/js/plugins/dataTables/dataTables.bootstrap4.min.js"));

            //Validation
            bundles.Add(new ScriptBundle("~/plugins/validation").Include(
                        "~/Content/js/plugins/jquery-validation/jquery.validate.min.js",
                        "~/Content/js/plugins/jquery-validation/additional-methods.min.js"));

            //Select
            bundles.Add(new ScriptBundle("~/plugins/select").Include(
                        "~/Content/js/plugins/select2/select2.full.min.js"));

            //Datapicker
            bundles.Add(new ScriptBundle("~/plugins/datepicker").Include(
                        "~/Content/js/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"));

            //MaskedInput
            bundles.Add(new ScriptBundle("~/plugins/maskedinput").Include(
                        "~/Content/js/plugins/masked-inputs/jquery.maskedinput.min.js",
                        "~/Content/js/plugins/masked-money/jquery.maskMoney.min.js"));

            //MaxLenght
            bundles.Add(new ScriptBundle("~/plugins/maxlenght").Include(
                        "~/Content/js/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js"));
            
            bundles.Add(new ScriptBundle("~/plugins/chartJs").Include(
                      "~/Content/js/plugins/chartjs/Chart.min.js"));
            //Chart
            bundles.Add(new ScriptBundle("~/plugins/chart").Include(
                        "~/Content/js/plugins/chartjs/Chart.bundle.min.js",
                        "~/Content/js/plugins/chartjs/utils.js",
                        "~/Content/js/plugins/easy-pie-chart/jquery.easypiechart.min.js"));

            //Calendar
            bundles.Add(new ScriptBundle("~/plugins/calendar").Include(
                        "~/Content/js/plugins/jquery-ui/jquery-ui.min.js",
                        "~/Content/js/plugins/moment/moment.min.js",
                        "~/Content/js/plugins/fullcalendar/fullcalendar.min.js", 
                        "~/Content/js/plugins/fullcalendar/locale-all.js"));

            //Colorpickier
            bundles.Add(new ScriptBundle("~/plugins/colorpicker").Include(
                        "~/Content/js/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"));

            // Footable
            bundles.Add(new ScriptBundle("~/plugins/footable").Include(
                        "~/Content/js/plugins/footable/footable.all.min.js"));

            // Footable
            bundles.Add(new ScriptBundle("~/plugins/clockpicker").Include(
                        "~/Content/js/plugins/clockpicker/clockpicker.js"));

            // Gauge
            bundles.Add(new ScriptBundle("~/plugins/gauge").Include(
                      "~/Content/js/plugins/gauge/gauge.js",
                      "~/Content/js/plugins/gauge/gauge.min.js"));

            // Sidebar
            bundles.Add(new ScriptBundle("~/pages/sidebar").Include(
                      "~/Content/js/pages/sidebar.js"));

            // FileInput
            bundles.Add(new ScriptBundle("~/plugins/inputfile").Include(
                      "~/Content/js/plugins/inputfile/fileinput.min.js",
                      "~/Content/js/plugins/inputfile/piecif.min.js",
                      "~/Content/js/plugins/inputfile/popper.min.js",
                      "~/Content/js/plugins/inputfile/purify.min.js",
                      "~/Content/js/plugins/inputfile/sortable.min.js"));

        }
    }
}
