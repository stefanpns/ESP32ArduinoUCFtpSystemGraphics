using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using WpfApp2;

namespace coreapp
{

    
    


    class ClientSender
    {

        private HttpRequestRW _postRequestTalk = new PostHttpRequestRW();

        private TalkBuffer _talkBuffer = new TalkBuffer();

        private ArduinoIW arduinoIW = new ArduinoIW();

        private bool waitingForTheChain = false;

        public ClientSender()
        {
            //_postRequestTalk.setup("http://192.168.137.83/");
        }



        public  async Task  sendCommand(string commandToSend, string ipAdress) {

            _postRequestTalk.setup("http://"+ipAdress+"/");

            //Console.WriteLine(commandToSend);

            string chainCommand = null;

            _talkBuffer.clear();

                    
            string response = await _postRequestTalk.SendCommandAndRecieveResult(commandToSend);
            //System.Diagnostics.Debug.WriteLine("response: \n" + response);
                    
            if ( response == null ) {
                return;
            }

            if ( _talkBuffer.init(response) == false )
            {
                WpfApp2.EditorPrinter.PrintStatus("Buffer overflow - Unhandled error 3.");
                return;
            }

            CommandObject res = null;
            res = arduinoIW.interpret(_talkBuffer);

            if ( res == null ) {
                ErrorsApp.print();
                return;
            }
                    

                            
            if ( res.isValid() ) {

                if ( res.isComplete()) {

                    if ( waitingForTheChain ) {

                                
                        if ( arduinoIW.getPreviousCommandObject() != null ) {

                            CommandObject previousCmdObject = arduinoIW.getPreviousCommandObject();

                                    
                                if ( previousCmdObject.checkChainCommandName(res.getName()) ) {

                                if ( previousCmdObject.checkChainCmdResult(res) == true) {
                                    previousCmdObject.print();
                                    WpfApp2.EditorPrinter.WriteLine(previousCmdObject.getChainSuccessMessage());
                                    WpfApp2.EditorPrinter.PrintStatus("Chaining OK");
                                } else {
                                    WpfApp2.EditorPrinter.WriteLine(previousCmdObject.getChainUnsuccessMessage());
                                    WpfApp2.EditorPrinter.PrintStatus("Chaining not OK");
                                }
                                WpfApp2.EditorPrinter.WriteLine("");


                            }else {
                                WpfApp2.EditorPrinter.WriteLine("The new command result is not compatible with the previous command result.");
                                WpfApp2.EditorPrinter.WriteLine("The chain cannot be continued.");
                                WpfApp2.EditorPrinter.WriteLine("The new command result is:");
                                res.print();
                                WpfApp2.EditorPrinter.WriteLine("");
                                WpfApp2.EditorPrinter.PrintStatus("Chaining not OK");
                            }
                        }else {
                            WpfApp2.EditorPrinter.WriteLine("Program.cs: Unhandled case 1");
                        }

                        waitingForTheChain = false;

                    } else if ( res.isWaitingForTheChain() ) {

                        WpfApp2.EditorPrinter.PrintStatus("Waiting for the chain");
                        waitingForTheChain = true;

                        arduinoIW.setPreviousCommandObject(res);
                        chainCommand = res.getChainCommand();
                        await sendCommand(chainCommand, ipAdress);
                        return;

                    } else {

                        WpfApp2.EditorPrinter.PrintStatus("Successful");
                        res.print();
                        WpfApp2.EditorPrinter.WriteLine("");
                    }



                } else
                {
                    WpfApp2.EditorPrinter.PrintStatus("Not completed");
                    WpfApp2.EditorPrinter.WriteLine("Program.cs: We have recieved not completed command result.");
                }

            } else  {

                WpfApp2.EditorPrinter.PrintStatus("Not valid");
                WpfApp2.EditorPrinter.WriteLine("Program.cs: We have recieved a command result that is not valid.");
                WpfApp2.EditorPrinter.WriteLine("The command was ["+res.getName()+"].");
                waitingForTheChain = false;
            }

            arduinoIW.setPreviousCommandObject(res);


             
        }



    }
}
