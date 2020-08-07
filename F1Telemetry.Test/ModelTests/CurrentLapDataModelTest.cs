using F1Telemetry.WPF.Model;
using FluentAssertions;
using System.Collections;
using System.Linq;
using System.Reflection;
using Xunit;

namespace F1Telemetry.Test.ModelTests
{
    public class CurrentLapDataModelTest
    {
        [Fact]
        public void Clear_Should_Clear_The_Instance()
        {
            var model = new CurrentLapDataModel();
            var expected = new CurrentLapDataModel();

            var type = model.GetType();

            var arrayFields = type.GetTypeInfo().DeclaredFields.Where(fi => fi.IsPublic && fi.FieldType.IsArray);

            foreach (var arrayField in arrayFields)
            {
                var arr = arrayField.GetValue(model) as IList;
                for (int i = 0; i < arr.Count; i++)
                {
                    arr[i] = 1.0;
                }
            }

            model.Clear();

            foreach (var arrayField in arrayFields)
            {
                var arr = arrayField.GetValue(model);
                arr.Should().BeEquivalentTo(arrayField.GetValue(expected), because: "clear should reset the instance.");
            }
        }
    }
}