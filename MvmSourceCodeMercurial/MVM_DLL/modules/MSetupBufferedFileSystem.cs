using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using System.Net.Sockets;
/*
<setup_buffered_file_system>
      <max_files>100</max_files>
      <write_buffer_start_bytes>1024</write_buffer_start_bytes>
      <write_buffer_increment_bytes>8*1024</write_buffer_increment_bytes>
      <write_buffer_max_bytes>128*1024</write_buffer_max_bytes>
      <read_buffer_bytes>128*1024</read_buffer_bytes>
    </setup_buffered_file_system>
*/
namespace MVM
{
    class MSetupBufferedFileSystem : MDbCommon, IModuleSetup
    {
        public override void Setup(XmlElement me, ModuleContext mc, List<IModuleRun> run)
        {
            string maxFilesStr = mc.SyntaxReadString(me.SelectNodeInnerText("./max_files"));
            string writeBufferStartBytesStr = mc.SyntaxReadString(me.SelectNodeInnerText("./write_buffer_start_bytes"));
            string writeBufferIncrementBytesStr = mc.SyntaxReadString(me.SelectNodeInnerText("./write_buffer_increment_bytes"));
            string writeBufferMaxBytesStr = mc.SyntaxReadString(me.SelectNodeInnerText("./write_buffer_max_bytes"));
            string readBufferBytesStr = mc.SyntaxReadString(me.SelectNodeInnerText("./read_buffer_bytes"));
            BufferedFileSystem bfs = new BufferedFileSystem(
                maxFilesStr.ToInt(),
                writeBufferStartBytesStr.ToInt(),
                writeBufferIncrementBytesStr.ToInt(),
                writeBufferMaxBytesStr.ToInt(), 
                readBufferBytesStr.ToInt()
                );
            bfs.mvm = mc.mvm;
            if (mc.globalContext.bfs != null) throw new Exception("Error can only setup_buffered_file_system once");
            mc.globalContext.bfs = bfs;
        }
    }


    

}
