using System;
using System.Collections.Generic;
namespace hbxpog
{
    /* hbxpog (enigma encoded with 3/14/15/9/2/6, ie Pi)
     * Inspired by Germany's Enigma machine, before and during WW2, and One Time Pad ciphers
     * Created by TJ Wolschon 4/4/2017
     * This program creates One Time Pads for use in encoding and decoding messages.
     * The program can also do the encoding and decoding of the messages if the user chooses.
     */
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(int.MaxValue);
            Console.Write("To generate OTP type (1)\nTo decode a message type (2)\nEncode a message with a custom OTP type (3)\nGenerate a less secure OTP type (4): ");//prompt user for which operation to do
            string genOrDecodeResp = Console.ReadLine();//store their response
            if(genOrDecodeResp == "1")//if they want to generate an OTP
            {
                GenerateOTP();
            }
            else if(genOrDecodeResp == "2")//if they want to decode a message
            {
                DecodeMessage();
            }
            else if(genOrDecodeResp == "3")//if they want to encode with a custom OTP
            {
                Console.Write("Enter the key, seperated by (/): ");//prompt for the key, seperated by /
                string key = Console.ReadLine();//read that into a string
                string[] keyAStr = key.Split('/');//split the key into a string array
                int[] keyAInt = new int[keyAStr.Length];//create an int array for the key
                for (int i = 0; i < keyAInt.Length; i++)
                {
                    keyAInt[i] = int.Parse(keyAStr[i]);//parse every string of the key array into the int array
                }
                EncryptMessage(keyAInt);
            }
            else if(genOrDecodeResp == "4")//similar to 1, generates a set list of 1million ints to choose a key from
            {
                PiOTP();
            }
            else//if they didn't enter anything correctly
            {
                Console.WriteLine("Incorrect entry, exiting the program...");//inform them that they're dumb
                return;//and close the program
            }
        }
        private static void DecodeMessage()//method to decode a message using an OTP
        {
            Console.Write("Write out the message, do not use spaces: ");//prompt for message
            string encodedMsg = Console.ReadLine();//store in a string
            Console.Write("Enter the key, seperated by (/): ");//prompt for the key, seperated by /
            string key = Console.ReadLine();//read that into a string
            char[] encodedMsgA = encodedMsg.ToCharArray();//transfer the encoded message into a char array so we can work with it
            string[] keyAStr = key.Split('/');//split the key into a string array
            int[] keyAInt = new int[keyAStr.Length];//create an int array for the key
            char[] decodedMsgA = new char[encodedMsgA.Length];//create a char array for the decoded message
            for(int i = 0; i < keyAInt.Length; i++)
            {
                keyAInt[i] = int.Parse(keyAStr[i]);//parse every string of the key array into the int array
            }
            for(int i = 0; i<keyAInt.Length; i++)
            {
                decodedMsgA[i] = Shift(encodedMsgA[i], keyAInt[i]);//run the shift method for each char, using the correct shift amt, store in the decoded message array
            }
            Console.Write("Message decoded, press enter to display...");//tell user that we've finished decoding the msg
            Console.ReadLine();//let them choose when to show it, incase they're spies and it's unsafe or something Kappa
            for(int i = 0; i < decodedMsgA.Length; i++)
            {
                Console.Write(decodedMsgA[i]);//write out the decoded message
            }
            Console.WriteLine("\nPress any key to exit...");//prompt for exit
            Console.ReadKey();//continues onto the main and drops out of the program
        }
        private static char Shift(char letterToShift, int amtToShift)//gets the new letter from the encoded letter and the amount it needs to shift
        {
            Dictionary<char, int> alphabetD = new Dictionary<char, int>();//dictionary to store the position of each letter in the next array, probably can be done simpler
            char[] alphabet = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };//alphabet array
            for(int i = 0; i < 26; i++)
            {
                alphabetD.Add(alphabet[i], i);//populates the dictionary with the position of each letter in the alphabet array
            }
            int initLetterPlace = alphabetD[letterToShift];//the initial place of the letter we need to shift, in the array
            int newLetterPlace = initLetterPlace - amtToShift;//it's new place after shifting it
            if (newLetterPlace < 0)//if it's below zero, wrap it around
            {
                newLetterPlace += 26;
            }
            if(newLetterPlace >= 26)//if it's above or at 26, wrap it around
            {
                newLetterPlace -= 26;
            }
            return alphabet[newLetterPlace];//return the new letter which has been encoded with the amount to shift it
        }
        private static void GenerateOTP()
        {
            Random rng = new Random();//setup a non cryptographically secure random object (I would like to make this crypto safe eventually)
            Console.WriteLine("How many characters is the message, do not count spaces.");//prompt for length of the message
            string characterStr = Console.ReadLine();//read in their response
            int character;//setup an int for it
            int.TryParse(characterStr, out character);//parse it
            int[] shiftAmt = new int[character];//setup an array to store the OTP that is the length of the message
            for(int i = 0; i<shiftAmt.Length; i++)
            {
                int randShift = rng.Next(0,26);//can shift anywhere from 0 to 25 letters, i.e. from itself, to the letter just before it
                shiftAmt[i] = randShift;
            }
            Console.WriteLine("OTP Generated, Press enter to print...");//tell user we've generated the OTP
            Console.ReadLine();//wait for them to acknowledge
            for(int i = 0; i<shiftAmt.Length; i++)
            {
                Console.Write(shiftAmt[i] + "/");//writes out the OTP seperated by slashes
            }
            Console.Write("\nWould you like to generate a message with this OTP now(Y/N): ");//prompt if they would like to generate a message within the program
            string yesNo = Console.ReadLine();//store their response
            if(yesNo == "y" || yesNo == "Y" || yesNo.ToUpper() == "YES")//check for common yes responses
            {
                EncryptMessage(shiftAmt);//go to the encrypt method, using the OTP we just generated
            }
            else { return; }//otherwise they entered no or something else, and close
        }
        private static void EncryptMessage(int[] shiftAmt)
        {
            int[] otpShiftAmt = shiftAmt;//store the OTP in a local array, just a habit not necessary i dont think
            char[] encodedMessage = new char[otpShiftAmt.Length];//create a char array for the final encoded message
            Console.WriteLine("Enter your message without spaces: ");//prompt for the message to encode
            string messageToEncode = Console.ReadLine();//store it in a string
            char[] messageToEncodeA = messageToEncode.ToCharArray();//convert the string into a char array so we can work on it
            for(int i = 0; i < encodedMessage.Length; i++)
            {
                encodedMessage[i] = Shift(messageToEncodeA[i], otpShiftAmt[i]*-1);//sends each letter of the char array
            }                                                                     //with the amt to shift from the OTP to be encoded
            Console.Write("Message encoded, press enter to display...");//tell user we've encoded their message
            Console.ReadLine();//allow them to choose when to show it
            for(int i =  0; i < encodedMessage.Length; i++)
            {
                Console.Write(encodedMessage[i]);//write out the entire encoded message
            }
            Console.Write("\nPress any key to exit...");//wait for them to exit on their own
            Console.ReadKey();//waits for a key press to exit
        }
        private static void PiOTP()
        {
            Random rng = new Random(314159265);//seed the random with PI
            int[] randArr = new int[276800000];//setup an array with a lot values in it
            Console.Clear();
            Console.WriteLine("Generating OTPs, please wait...");
            for (int i = 0; i < 276800000; i++)//populate
            {
                int random = rng.Next(0, 26);//with ints 0-25
                randArr[i] = random;//store in the array
            }
            Console.Clear();
            bool stayInLoop = true;
            int charNum;
            int seqNum;
            do
            {

                Console.Write("How many characters: ");//prompt for how long the key should be
                charNum = int.Parse(Console.ReadLine());//parse that into an int

                Console.Write("Enter a number in the sequence(0-276800000): ");//prompt for where in the sequence, any one number will always return the same set
                int.TryParse(Console.ReadLine(), out seqNum);
                if (seqNum > 276800000 || charNum + seqNum > 276800000 || seqNum == 0)//if outside of the array
                {
                    Console.WriteLine("Please choose a lower number in the sequence...");//call user out
                    Console.ReadKey();
                    stayInLoop = true;
                }
                else { stayInLoop = false; }
            } while (stayInLoop);

            int[] shiftAmt = new int[charNum];//setup an array to store the OTP that is the length of the message
            int j = 0;//indexer
            for (int i = seqNum; i < seqNum + charNum; i++)//this loop gets an exact sequence from the large array of randoms
            {
                shiftAmt[j] = randArr[i];
                j++;
                if (i == seqNum + charNum - 1)
                {
                    Console.Write(randArr[i] + "\n");
                }
                else
                {
                    Console.Write(randArr[i] + "/");
                }
            }

            Console.Write("\nWould you like to generate a message with this OTP now(Y/N): ");//prompt if they would like to generate a message within the program
            string yesNo = Console.ReadLine();//store their response
            if (yesNo == "y" || yesNo == "Y" || yesNo.ToUpper() == "YES")//check for common yes responses
            {
                EncryptMessage(shiftAmt);//go to the encrypt method, using the OTP we just generated
            }
            else
            {

                Console.Write("Press any key to exit program...");
                Console.ReadKey();
            }
        }

    }
}
