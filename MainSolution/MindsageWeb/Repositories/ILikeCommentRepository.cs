using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Like comment
    /// </summary>
    public interface ILikeCommentRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูลการ like comment จากรหัส lesson
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        IEnumerable<LikeComment> GetLikeCommentByLessonId(string lessonId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void UpsertLikeComment(LikeComment data);

        #endregion Methods
    }
}
