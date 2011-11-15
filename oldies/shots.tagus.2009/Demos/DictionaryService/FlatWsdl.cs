using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml.Schema;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;
using Message = System.Web.Services.Description.Message;
using PortType = System.Web.Services.Description.PortType;
using Import = System.Web.Services.Description.Import;
using WsdlBinding = System.Web.Services.Description.Binding;
using System;
using System.ServiceModel.Channels;

namespace Thinktecture.ServiceModel.Extensions.Description
{
    public class FlatWsdl : IWsdlExportExtension, IEndpointBehavior
    {
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            XmlSchemaSet schemaSet = exporter.GeneratedXmlSchemas;

            foreach (ServiceDescription wsdl in exporter.GeneratedWsdlDocuments)
            {
                wsdl.
                List<Import> importList = new List<Import>();
                Console.WriteLine("\n-- Service Description --");
                wsdl.Write(Console.Out);
                foreach (Import imp in wsdl.Imports) {
                    Console.WriteLine("Removing import");
                    ServiceDescription sd = imp.;
                    Console.WriteLine(sd);
                    foreach (Message msg in sd.Messages) {
                        wsdl.Messages.Add(msg);
                    }
                    foreach (PortType pt in sd.PortTypes) {
                        wsdl.PortTypes.Add(pt);
                    }
                    foreach(XmlSchema schema in sd.Types.Schemas){
                        wsdl.Types.Schemas.Add(schema);
                    }
                    foreach (WsdlBinding bind in sd.Bindings) {
                        //wsdl.Bindings.Add(bind);
                        bind.ToString();
                    }
                    //wsdl.Imports.Remove(imp);
                    importList.Add(imp);
                }

                foreach (var imp in importList) {
                    wsdl.Imports.Remove(imp);
                }

                
                List<XmlSchema> importsList = new List<XmlSchema>();

                foreach (XmlSchema schema in wsdl.Types.Schemas)
                {
                    AddImportedSchemas(schema, schemaSet, importsList);
                }

                wsdl.Types.Schemas.Clear();

                foreach (XmlSchema schema in importsList)
                {
                    RemoveXsdImports(schema);
                    wsdl.Types.Schemas.Add(schema);
                }
            }
        }

        private void AddImportedSchemas(XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importsList)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas =
                    schemaSet.Schemas(import.Namespace);

                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!importsList.Contains(ixsd))
                    {
                        importsList.Add(ixsd);
                        AddImportedSchemas(ixsd, schemaSet, importsList);
                    }
                }
            }
        }

        private void RemoveXsdImports(XmlSchema schema)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                if (schema.Includes[i] is XmlSchemaImport)
                    schema.Includes.RemoveAt(i--);
            }
        }
        
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}