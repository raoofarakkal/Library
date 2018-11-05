using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Library2.Cs.Net
{
    public class Emailer
    {
        string mSMTPserver = "";
        string mFrom = "";
        string mTo = "";
        string mCc = "";
        string mBCc = "";
        string mSubj = "";
        string mBody = "";
        bool mBodyHtml = true;
        //MUST BE AVAILABLE IN WEB SERVER
        string mFileAttachments = "";

        public string _MailServer
        {
            get { return mSMTPserver; }
            set { mSMTPserver = value; }
        }

        public string _From
        {
            get { return mFrom; }
            set { mFrom = value; }
        }

        public string _To
        {
            get { return mTo; }
            set { mTo = value; }
        }

        public string _Cc
        {
            get { return mCc; }
            set { mCc = value; }
        }

        public string _BCc
        {
            get { return mBCc; }
            set { mBCc = value; }
        }

        public string _Subject
        {
            get { return mSubj; }
            set { mSubj = value; }
        }

        public string _Body
        {
            get { return mBody; }
            set { mBody = value; }
        }

        public bool _BodyIsHTML
        {
            get { return mBodyHtml; }
            set { mBodyHtml = value; }
        }

        public string _FileAttachments
        {
            get { return mFileAttachments; }
            set { mFileAttachments = value; }
        }

        public bool _Send()
        {
            bool mRet = false;
            try
            {
                string[] mmTo = mTo.Split(';');// Strings.Split(mTo, ";");
                string[] mmCc = mCc.Split(';');// Strings.Split(mCc, ";");
                string[] mmBCc = mBCc.Split(';');// Strings.Split(mBCc, ";");
                string[] mAttachMents = mFileAttachments.Split(';');// Strings.Split(mFileAttachments, ";");

                MailMessage mMail = new MailMessage();

                //FROM
                MailAddress mAdd = new MailAddress(mFrom);
                mMail.From = mAdd;
                mAdd = null;

                //TO
                for (int cnt = 1; cnt <= mmTo.Length; cnt++)
                {
                    if (!string.IsNullOrEmpty(mmTo[cnt - 1]))
                    {
                        mAdd = new MailAddress(mmTo[cnt - 1]);
                        mMail.To.Add(mAdd);
                        mAdd = null;
                    }
                }

                //CC
                for (int cnt = 1; cnt <= mmCc.Length; cnt++)
                {
                    if (!string.IsNullOrEmpty(mmCc[cnt - 1]))
                    {
                        mAdd = new MailAddress(mmCc[cnt - 1]);
                        mMail.CC.Add(mAdd);
                        mAdd = null;
                    }
                }

                //BCC
                for (int cnt = 1; cnt <= mmBCc.Length; cnt++)
                {
                    if (!string.IsNullOrEmpty(mmBCc[cnt - 1]))
                    {
                        mAdd = new MailAddress(mmBCc[cnt - 1]);
                        mMail.Bcc.Add(mAdd);
                        mAdd = null;
                    }
                }

                Attachment mAttachment = default(Attachment);
                for (int cnt = 1; cnt <= mAttachMents.Length; cnt++)
                {
                    if (!string.IsNullOrEmpty(mAttachMents[cnt - 1]))
                    {
                        mAttachment = new Attachment(mAttachMents[cnt - 1]);
                        mMail.Attachments.Add(mAttachment);
                        mAttachment = null;
                    }
                }

                mMail.Subject = mSubj;
                mMail.Body = mBody;
                mMail.IsBodyHtml = mBodyHtml;

                SmtpClient mClient = new SmtpClient();
                mClient.Host = mSMTPserver;
                mClient.Port = 25;
                mClient.UseDefaultCredentials = true;
                mClient.Send(mMail);
                mMail.Dispose();
                mMail = null;
                mClient = null;


                mRet = true;
            }
            catch (FormatException ex)
            {
                mRet = false;
                throw ex;
            }
            catch (SmtpException ex)
            {
                mRet = false;
                throw ex;
            }
            return mRet;
        }
    }


}
