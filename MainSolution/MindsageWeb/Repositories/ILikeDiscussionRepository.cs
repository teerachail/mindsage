using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    public interface ILikeDiscussionRepository
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discussionId"></param>
        /// <returns></returns>
        IEnumerable<LikeDiscussion> GetLikeDiscussionByDiscusionId(string discussionId);
        void UpsertLikeDiscussion(LikeDiscussion item);

        #endregion Methods
    }
}
