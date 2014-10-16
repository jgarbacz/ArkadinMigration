using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;

namespace MVM
{
    class MSpawn : IModuleSetup, IModuleRun
    {
        /*
         * <spawn>
         * <internal_type>'string_hash'|'int_hash'|'format_array'|delta_format_array'</internal_type>   // setuptime
         * <object_type>ACCOUNT</object_type>                                       // runtime
         * <object_id>OBJECT.object_id</object_id>                                  // runtime
         * <inherit_parent>TEMP.parent_oid</inherit_parent>                         // runtime
         * <field name='a'>'a value'</field>
         * <field name='b'>'b value'</field>
         * <pin_to_proc>1</pin_to_proc>
         * <delta_tracking_support>1|0</delta_tracking_support>
         * <delta_tracking_on>1|0</delta_tracking_on>
         * </spawn>
         */

        string objectType;
        string objectId;
        string internalTypeSyntax;
        string inheritParent;
        string pinToProc;
        string deltaTrackingSupportSyntax;
        string feedbackNameSyntax;

        IWriteString objectIdParsed;
        IReadString objectTypeParsed;
        IReadString internalTypeParsed;
        IReadString inheritParentParsed;
        IReadString pinToProcParsed;
        IReadString deltaTrackingSupportParsed;
       // IReadString deltaTrackingOnParsed;
        IReadString feedbackNameParsed;
        
        public void Setup(System.Xml.XmlElement me,ModuleContext mc, List<IModuleRun> run)
        {
            MSpawn m = new MSpawn();
            // xml extraction
            m.objectType = me.SelectNodeInnerText("./object_type");
            m.objectId = me.SelectNodeInnerText("./object_id");
            m.internalTypeSyntax = me.SelectNodeInnerText("./internal_type");
            m.inheritParent = me.SelectNodeInnerText("./inherit_parent");
            m.pinToProc = me.SelectNodeInnerText("./pin_to_proc", "1");
            m.deltaTrackingSupportSyntax = me.SelectNodeInnerText("./delta_tracking_support","0");
            m.feedbackNameSyntax = me.SelectNodeInnerText("./feedback_name", "''");

            // parsing
            m.objectTypeParsed = mc.ParseSyntax(m.objectType);
            if (m.objectId != null) m.objectIdParsed = mc.ParseWritableSyntax(m.objectId);
            if (m.inheritParent != null) m.inheritParentParsed = mc.ParseSyntax(m.inheritParent);
            if (m.pinToProc != null) m.pinToProcParsed = mc.ParseSyntax(m.pinToProc);
            m.deltaTrackingSupportParsed = mc.ParseSyntax(m.deltaTrackingSupportSyntax);
            m.feedbackNameParsed = mc.ParseSyntax(m.feedbackNameSyntax);
            if (m.internalTypeSyntax != null) m.internalTypeParsed = mc.ParseSyntax(m.internalTypeSyntax);
           
            // runtime
            run.Add(m);
        }

        public void Run(ModuleContext mc)
        {
            string objectTypeValue=this.objectTypeParsed.Read(mc);
            string inheritParentValue = this.inheritParentParsed != null ? this.inheritParentParsed.Read(mc) : "0";
            string pinToProcValue = this.pinToProcParsed != null ? this.pinToProcParsed.Read(mc) : "1";
            bool deltaTrackingSupport = this.deltaTrackingSupportParsed.Read(mc).Equals("1");
            string objectId;
            
            string internalType =
                this.internalTypeParsed != null
                ? this.internalTypeParsed.Read(mc)
                : (deltaTrackingSupport ? "format_array_delta" : "string_hash");


            switch (internalType)
            {
                case "string_hash":
                    {
                        objectId = mc.Spawn(objectTypeValue);
                        break;
                    }
                case "format_array":
                    {
                        string feedbackName = this.feedbackNameParsed.Read(mc);
                        objectId = mc.SpawnObjectDataFormatted(objectTypeValue, feedbackName);
                        break;
                    }
                case "format_array_delta":
                    {
                        string feedbackName = this.feedbackNameParsed.Read(mc);
                        objectId = mc.SpawnObjectDataFormattedDelta(objectTypeValue, feedbackName);
                        break;
                    }
                //case "string_hash_delta":
            //{
                //        objectId = mc.SpawnObjectDataDelta(objectTypeValue);
                //        break;
            //}
                default:
                    {
                        throw new Exception("Invalid internal_type:" + internalType);
                    }
            }


            if (objectIdParsed != null)
            {
                objectIdParsed.Write(mc,objectId);
                if (pinToProcValue.Equals("1"))
                {
                    mc.procInst.RegisterSpawnedOid(objectId);
                }
            }

            if (inheritParentValue.Equals("1"))
            {
                mc.InheritObject(mc.objectData.objectId, objectId);
            }
        }
    }
}
