using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace Library2.Cs.Net
{
    public class TcpIP
    {
        TcpClient mTcpClient;
        NetworkStream mNetStream;
        string mIPAddress;
        int mPort;
        int mTimeOut = 20;

        public TcpIP(string pIPaddress, int pPort)
        {
            mIPAddress = pIPaddress;
            mPort = pPort;
        }

        public int TimeOut
        {
            get { return mTimeOut; }
            set { mTimeOut = value; }
        }

        public bool Connected
        {
            get
            {
                if (mTcpClient != null)
                {
                    return mTcpClient.Connected;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Open()
        {
            if (!Connected)
            {
                try
                {
                    mTcpClient = new TcpClient();
                    mTcpClient.Connect(mIPAddress, mPort);
                    mNetStream = mTcpClient.GetStream();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Close()
        {
            if (Connected)
            {
                mTcpClient.Close();
            }
            return true;
        }

        public bool Send(string pRequest)
        {
            if (Connected)
            {
                string mStr = null;
                byte[] mBytes = null;
                mStr = pRequest + "\r\n";
                mBytes = Encoding.ASCII.GetBytes(mStr);
                mNetStream.Write(mBytes, 0, mBytes.Length);
                return true;
            }
            else
            {
                throw new Exception("Not connected");
            }
        }

        public string Receive()//1 ok
        {
            return Receive("");
        }

        public string Receive(string ReadUntil)//1 ok
        {
            if (Connected)
            {
                byte[] mReadBuffer = new byte[mTcpClient.ReceiveBufferSize + 1];
                StringBuilder mInData = new StringBuilder();
                int mBytesRead = 0;
                long mSec = 0;
                try
                {
                    mSec = (System.DateTime.Now.Ticks / 10000000);
                    if (mNetStream.CanRead)
                    {
                        while (string.IsNullOrEmpty(mInData.ToString()))
                        {
                            //while (mNetStream.DataAvailable)
                            while (true)
                            {
                                mBytesRead = mNetStream.Read(mReadBuffer, 0, mReadBuffer.Length);
                                //mInData.AppendFormat("{0}", Encoding.ASCII.GetString(mReadBuffer, 0, mBytesRead));
                                mInData.AppendFormat("{0}", Encoding.UTF8.GetString(mReadBuffer, 0, mBytesRead));
                                if ((System.DateTime.Now.Ticks / 10000000) > mSec + mTimeOut)
                                {
                                    break;
                                }
                                if (!string.IsNullOrEmpty(ReadUntil))
                                {
                                    if (mInData.ToString().IndexOf(ReadUntil, 0) > 1)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!mNetStream.DataAvailable)
                                    {
                                        break;
                                    }
                                }

                            }
                            if ((System.DateTime.Now.Ticks / 10000000) > mSec + mTimeOut)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return mInData.ToString();
            }
            else
            {
                throw new Exception("Not connected");
            }


        }

    }

}
