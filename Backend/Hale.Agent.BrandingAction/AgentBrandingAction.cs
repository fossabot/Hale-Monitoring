using System;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;

namespace AgentBrandingAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult AgentBrandingAction(Session session)
        {
            try
            {
                session.Log("Begin Agent Branding.");

                var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Hale", "Agent");
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                session.Log("Writing core-keys.xml...");
                using (var sw = File.CreateText(Path.Combine(basePath, "core-keys.xml")))
                {
                    session.Log(session["HALE_CORE_KEY"]);
                    sw.Write(session["HALE_CORE_KEY"]);
                }

                session.Log("Writing agent-keys.xml...");
                using (var sw = File.CreateText(Path.Combine(basePath, "agent-keys.xml")))
                {
                    session.Log(session["HALE_AGENT_KEYS"]);
                    sw.Write(session["HALE_AGENT_KEYS"]);
                }

                session.Log("Writing nemesis.yaml...");
                using (var sw = File.CreateText(Path.Combine(basePath, "nemesis.yaml")))
                {
                    var nemesisConf = session["HALE_AGENT_NEMESIS_CONFIG"];
                    nemesisConf = nemesisConf.Replace("<HOSTNAME>", session["HALE_CORE_HOSTNAME"]);
                    nemesisConf = nemesisConf.Replace("<SENDPORT>", session["HALE_CORE_PORT_SEND"]);
                    nemesisConf = nemesisConf.Replace("<RECEIVEPORT>", session["HALE_CORE_PORT_RECEIVE"]);
                    nemesisConf = nemesisConf.Replace("<ENCRYPTION>", session["HALE_CORE_ENCRYPTION"]);
                    nemesisConf = nemesisConf.Replace("<GUID>", string.IsNullOrEmpty(session["HALE_AGENT_GUID"]) ?
                        Guid.NewGuid().ToString() : session["HALE_AGENT_GUID"]);
                    session.Log(nemesisConf);
                    sw.Write(nemesisConf);
                }

                session.Log("Writing config.yaml...");
                using (var sw = File.CreateText(Path.Combine(basePath, "config.yaml")))
                {
                    sw.Write("modules:");
                }

            }
            catch (Exception e)
            {
                session.Log("Agent Branding failed:\n" + e.Message);
                return ActionResult.Failure;
            }

            session.Log("Agent Branding completed.");
            return ActionResult.Success;
        }
    }
}
