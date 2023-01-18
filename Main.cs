using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_walls_volume
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> elementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Face, "Выберите стены");
            var wallList = new List<Wall>();
            double totalVolume=0;
            
            foreach (var selectedElement in elementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Wall oWall = (Wall)element;
                    Parameter volume = oWall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    totalVolume += volume.AsDouble();
                }
            }
            totalVolume = UnitUtils.ConvertFromInternalUnits(totalVolume, UnitTypeId.CubicMeters);
            TaskDialog.Show("Volume", $"Объем выбранных стен: {totalVolume} м3");
           

            return Result.Succeeded;

        }
    }
}
