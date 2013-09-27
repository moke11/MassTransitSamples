using System;
using System.Text;
using MassTransit;

namespace RabbitCommon
{
    public static class InspectorGadget
    {
        public static void WriteDetails(IServiceBus bus)
        {
            var inspectText = new StringBuilder();
            inspectText.AppendLine("InboundPipeline:");
            inspectText.AppendLine("");

            var inboundInspector = new MassTransit.Pipeline.Inspectors.PipelineViewer();
            bus.InboundPipeline.Inspect(inboundInspector);
            inspectText.Append(inboundInspector.Text);
            inspectText.AppendLine("OutboundPipeline:\r\n");
            inspectText.AppendLine("");

            var outboundInspector = new MassTransit.Pipeline.Inspectors.PipelineViewer();
            bus.OutboundPipeline.Inspect(outboundInspector);
            inspectText.Append(outboundInspector.Text);

            Console.WriteLine(inspectText);
            // end inspect
        }
    }
}
