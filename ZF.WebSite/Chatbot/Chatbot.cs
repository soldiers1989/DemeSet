using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMLbot;

namespace Chatbot
{
    public class ChatRobot
    {
        const string UserId = "CityU.Scm.David";
        private Bot AimlBot;
        private User myUser;

        public ChatRobot( )
        {
            AimlBot = new Bot( );
            
            AimlBot.UpdatedAimlDirectory = "ChatbotLib\\aiml";
            AimlBot.UpdatedConfigDirectory = AppDomain.CurrentDomain.BaseDirectory + "ChatbotLib\\config";
            AimlBot.GlobalSettings.addSetting( "stripperregex", "[^\u4e00-\u9fa5|a-zA-Z0-9]" ); //设置正则表达式,支持中文和英文
              //< item name = "stripperregex" value = "[^0-9a-zA-Z]" />只支持英文配置 ,配置文件中不能有注释，否则报错
                myUser = new User( UserId, AimlBot );

            Initialize( );
        }

        // Loads all the AIML files in the\AIML folder        
        public void Initialize( )
        {
            AimlBot.loadSettings();
            
            AimlBot.isAcceptingUserInput = false;

            AimlBot.loadAIMLFromFiles( );
            AimlBot.isAcceptingUserInput = true;
        }

        // Given an input string, finds aresponse using AIMLbot lib
        public String getOutput( String input )
        {
            Request r = new Request( input, myUser, AimlBot );
            Result res = AimlBot.Chat( r );
            return ( res.Output );
        }
    }


   
}
