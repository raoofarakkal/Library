//http://msdn.microsoft.com/en-us/magazine/cc135979.aspx


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;

namespace Library2.Cs.Sys
{


    /// <summary>
    /// ActiveDirectory Manager
    /// </summary>
    public class ActiveDirectory{
        private string mPwd;
        private string mDomain;
        private string mLoginID;
        private PrincipalContext mAD;

        public ActiveDirectory()
        {
        }

        public ActiveDirectory(string Domain,string PrincipalContextContainer, string LoginID, string Password)
        {
            init(Domain, PrincipalContextContainer,LoginID, Password);
        }

        private void init(string Domain, string PrincipalContextContainer, string LoginID, string Password )
        {
            mPwd = Password;
            mDomain = Domain;
            mLoginID = LoginID;
            if (!string.IsNullOrWhiteSpace(PrincipalContextContainer)) 
            {
                mAD = new PrincipalContext(ContextType.Domain, mDomain,PrincipalContextContainer, mLoginID, mPwd);
            }else{
                mAD = new PrincipalContext(ContextType.Domain, mDomain, mLoginID, mPwd);
            }

            //mAD = new PrincipalContext(ContextType.Machine, "01l-arakkala");
        }

        public void Dispose()
        {
            mAD.Dispose();
        }

        public string Domain
        {
            get { return mDomain; }
        }

        public string LoginID
        {
            get { return mLoginID; }
        }

    
        public PrincipalContext principalContext
        {
            get { return mAD; }
        }

        #region U S E R S

        public UserPrincipal getUser(string pSamAccountName)
        {
            UserPrincipal mUser = new UserPrincipal(mAD);
            mUser = UserPrincipal.FindByIdentity(mAD,IdentityType.SamAccountName, pSamAccountName);
            return mUser;
        }

        public IEnumerable<UserPrincipal> getUsers(string pSearchText)
        {
            List<UserPrincipal> mRet = null;
            using (UserPrincipal mUser = new UserPrincipal(mAD))
            {
                if (!string.IsNullOrEmpty(pSearchText))
                {
                    mUser.Name = SearchKeywordSet(pSearchText);
                    //mUser.SamAccountName = SearchKeywordSet(pSearchText);
                }
                using (PrincipalSearcher mSearch = new PrincipalSearcher(mUser))
                {

                    mSearch.QueryFilter = mUser;
                    mRet = (from principal in mSearch.FindAll() select principal as UserPrincipal).ToList();
                    mSearch.Dispose();
                }
                mUser.Dispose();
            }
            return mRet;
        }

        #endregion

        #region G R O U P S

        public GroupPrincipal getGroup(string pGroupName)
        {
            GroupPrincipal mGroup = new GroupPrincipal(mAD);
            mGroup = GroupPrincipal.FindByIdentity(mAD, IdentityType.SamAccountName, pGroupName);
            return mGroup;
        }

        public IEnumerable<GroupPrincipal> getGroups(string pSearchText)
        {
            List<GroupPrincipal> mRet = null;
            using (GroupPrincipal mGroup = new GroupPrincipal(mAD))
            {
                if (!string.IsNullOrEmpty(pSearchText))
                {
                    mGroup.Name = SearchKeywordSet(pSearchText);
                }
                using (PrincipalSearcher mSearch = new PrincipalSearcher(mGroup))
                {
                    mSearch.QueryFilter = mGroup;
                    mRet = (from SearchResults in mSearch.FindAll() select SearchResults as GroupPrincipal).ToList();
                    mSearch.Dispose();
                }
                mGroup.Dispose();
            }
            return mRet;
        }

        public IEnumerable<GroupPrincipal> getUserGroups(string pSamAccountName)
        {
            List<GroupPrincipal> mRet = null;
            UserPrincipal mUser = getUser(pSamAccountName);
            mRet = (from mGrps in _getGroups(mUser) select mGrps as GroupPrincipal).ToList();
            return mRet;
        }

        public List<string> getUserGroupNames(string pSamAccountName)
        {
            List<string> mRet = null;
            UserPrincipal mUser = getUser(pSamAccountName);
            mRet = _getGroups(mUser).Select(r => r.Name).ToList();
            return mRet;
        }

        public bool isUserExistInGroup(string pSamAccountName, string pGroupName)
        {
            bool mRet = false;
            UserPrincipal mUser = getUser(pSamAccountName);
            mRet = _getGroups(mUser).Select(r => r.Name).ToList().Contains(pGroupName, StringComparer.CurrentCultureIgnoreCase);
            return mRet;
        }

        public bool isUserExistInGroups(string pSamAccountName, List<string> pGroupNames)
        {
            bool mRet = false; 
            UserPrincipal mUser = getUser(pSamAccountName);
            List<string> mGroups = _getGroups(mUser).Select(r => r.Name).ToList();// .GetAuthorizationGroups().Select(r => r.Name).ToList();
            foreach (string mGrp in pGroupNames)
            {
                mRet = mGroups.Contains(mGrp, StringComparer.CurrentCultureIgnoreCase);
                if (mRet)
                {
                    break;
                }
            }
            return mRet;
        }

        public IEnumerable<GroupPrincipal> getUserGroups(UserPrincipal pUser)
        {
            List<GroupPrincipal> mRet = null;
            mRet = (from mGrps in _getGroups(pUser) select mGrps as GroupPrincipal).ToList();
            return mRet;
        }

        private PrincipalSearchResult<Principal> _getGroups(UserPrincipal pUser)
        {
            PrincipalSearchResult<Principal> mRet = null;
            try
            {
                mRet = pUser.GetGroups();
            }
            catch (Exception)
            {
                mRet = pUser.GetAuthorizationGroups();
            }
            return mRet;
        }

        #endregion

        #region PRIVATE

        private string SearchKeywordSet(string pKey)
        {
            if (pKey.Contains("*"))
            {
                return pKey;
            }
            else
            {
                return string.Format("*{0}*", pKey);
            }
        }

        #endregion

    }

}
