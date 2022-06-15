using Automation.BDaq;

namespace IoCardWebApi
{
    public class DiCard
    {
        private static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }//BioFailed

        private static string[] GetCardValueStringArray(InstantDiCtrl diCardInstant, int totalPort)
        {
            ErrorCode errorCode = ErrorCode.Success;

            int startPort = 0;

            int portCount = totalPort / 8;
            byte[] buffer = new byte[totalPort * 2];
            string[] values = new string[totalPort];

            ////32 ports
            //int portCount = 4;
            //byte[] buffer = new byte[64];
            //string[] values = new string[32];

            errorCode = diCardInstant.Read(startPort, portCount, buffer);
            if (BioFailed(errorCode))
            {
                throw new Exception();
            }
            else
            {
                for (int i = 0; i < portCount; ++i)
                {
                    string bs = Convert.ToString(buffer[i], 2).PadLeft(8, '0');
                    for (int j = 0; j < bs.Length; j++)
                    {
                        values[i * 8 + j] = bs.Substring(bs.Length - j - 1, 1);
                    }
                }
            }
            return values;
        }//GetCardValueStringArray

        public static string[]? GetSignal(string cardID, int totalPort)
        {
            InstantDiCtrl diCardInstant = new InstantDiCtrl() { SelectedDevice = new DeviceInformation(cardID) };

            if (diCardInstant.Initialized)
            {
                return GetCardValueStringArray(diCardInstant, totalPort);
            }
            else
            {
                return null;
            }
        }
    }
}
