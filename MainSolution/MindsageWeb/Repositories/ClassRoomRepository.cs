using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindsageWeb.Repositories.Models;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Class room
    /// </summary>
    public class ClassRoomRepository : IClassRoomRepository
    {
        #region IClassRoomRepository members

        /// <summary>
        /// ขอข้อมูล Class room จากรหัส
        /// </summary>
        /// <param name="id">รหัส Class room ที่ต้องการขอ</param>
        public ClassRoom GetClassRoomById(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        public void UpdateClassRoom(ClassRoom data)
        {
            throw new NotImplementedException();
        }

        #endregion IClassRoomRepository members
    }
}
