using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BackToA1
{
    public partial class BackToA1
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.WorkbookBeforeClose += FocusOnA1BeforeClose;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void FocusOnA1BeforeClose(Excel.Workbook Wb, ref bool Cancel)
        {
            if (Wb.ReadOnly)
            {
                return;
            }

            var stage = "";
            var wasSaved = false;
            var currentActiveSheet = Wb.ActiveSheet as Excel.Worksheet;

            try
            {
                stage = "Counting sheets";
                if (Wb.Worksheets.Count == 0)
                {
                    return;
                }

                wasSaved = Wb.Saved;

                // The Select() method requires the workbook to be active.
                stage = "Activating workbook";
                Wb.Activate();

                var currentProcessingSheet = 0;
                foreach (var worksheet in Wb.Worksheets.OfType<Excel.Worksheet>())
                {
                    if (worksheet.Visible == Excel.XlSheetVisibility.xlSheetHidden)
                    {
                        stage = $"Skipping hidden sheet {currentProcessingSheet} of {Wb.Worksheets.Count}";
                        continue;
                    }

                    stage = $"Moving focus to A1 on sheet {currentProcessingSheet} of {Wb.Worksheets.Count}";
                    worksheet.Activate();
                    worksheet.Range["A1"].Select();

                    stage = $"Resetting zoom on sheet {currentProcessingSheet} of {Wb.Worksheets.Count}";
                    Wb.Application.ActiveWindow.Zoom = 100;

                    currentProcessingSheet++;
                }

                if (currentActiveSheet != null)
                {
                    currentActiveSheet.Activate();
                }

                if (wasSaved && !string.IsNullOrWhiteSpace(Wb.Path))
                {
                    Wb.Save();
                }
            }
            catch (COMException ex)
            {
                Log($"Stage={stage}; HRESULT=0x{ex.ErrorCode:X8}; Exception={ex}");
            }
        }

        private static void Log(string message)
        {
            try
            {
                Trace.TraceError(message);
            } catch
            {
                // Ignored on purpose, in order to not crash the close process
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
