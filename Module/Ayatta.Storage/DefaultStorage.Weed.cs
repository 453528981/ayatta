using Dapper;
using Ayatta.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        #region Ŀ¼ �ļ�       

        ///<summary>
        /// Ŀ¼ ����
        ///</summary>
        ///<param name="o">WeedDir</param>
        ///<returns></returns>
        public int WeedDirCreate(WeedDir o)
        {
            return Try(nameof(WeedDirCreate), () =>
            {
                var cmd = SqlBuilder.Insert("WeedDir")
                .Column("Pid", o.Pid)
                .Column("Name", o.Name)
                .Column("Depth", o.Depth)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("UserId", o.UserId)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy ?? string.Empty)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy??string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return WeedConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// Ŀ¼ ����
        ///</summary>
        ///<param name="o">WeedDir</param>
        ///<returns></returns>
        public bool WeedDirUpdate(WeedDir o)
        {
            return Try(nameof(WeedDirUpdate), () =>
            {
                var cmd = SqlBuilder.Update("WeedDir")
                .Column("Name", o.Name)
                .Column("Depth", o.Depth)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                //.Column("UserId", o.UserId)
                .Column("Status", o.Status)
                //.Column("CreatedBy", o.CreatedBy)
                //.Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return WeedConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// Ŀ¼ ״̬ ����
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool WeedDirStatusUpdate(int id, bool status)
        {
            return Try(nameof(WeedDirStatusUpdate), () =>
            {
                var sql = @"update WeedDir set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return WeedConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// Ŀ¼ ɾ��
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool WeedDirDelete(int id)
        {
            //return Try(nameof(WeedDirDelete), () =>
            //{
            //    var sql = @"delete from WeedDir where Pid=@id;delete from AdModule where id=@id;";
            //    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            //    return BaseConn.Execute(cmd) > 0;
            //});
            return false;
        }

        ///<summary>
        /// Ŀ¼ ��ȡ
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public WeedDir WeedDirGet(int id)
        {
            return Try(nameof(WeedDirGet), () =>
            {
                var sql = @"select * from WeedDir where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return WeedConn.QueryFirstOrDefault<WeedDir>(cmd);
            });
        }

        public IList<Magic<int, int, string>> WeedDirList(int userId)
        {
            return Try(nameof(WeedDirGet), () =>
            {
                var sql = @"select id as First,pid as Second,name as Third from WeedDir where UserId=@userId";
                var cmd = SqlBuilder.Raw(sql, new { userId }).ToCommand();
                return WeedConn.Query<Magic<int, int, string>>(cmd).ToList();
            });
        }

        ///<summary>
        /// �ļ� ����
        ///</summary>
        ///<param name="o">WeedFile</param>
        ///<returns></returns>
        public int WeedFileCreate(WeedFile o)
        {
            return Try(nameof(WeedFileCreate), () =>
            {
                var cmd = SqlBuilder.Insert("WeedFile")
                .Column("Id", o.Id)
                .Column("Uid", o.Uid)
                .Column("Did", o.Did)
                .Column("Ext", o.Ext)
                .Column("Url", o.Url)
                .Column("Size", o.Size)
                .Column("Name", o.Name)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy ?? string.Empty)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy ?? string.Empty)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return WeedConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// �ļ� ����
        ///</summary>
        ///<param name="o">WeedFile</param>
        ///<returns></returns>
        public bool WeedFileUpdate(WeedFile o)
        {
            return Try(nameof(WeedFileUpdate), () =>
            {
                var cmd = SqlBuilder.Update("WeedFile")
                .Column("Name", o.Name)
                .Column("Badge", o.Badge ?? string.Empty)
                .Column("Extra", o.Extra ?? string.Empty)
                //.Column("UserId", o.UserId)
                .Column("Status", o.Status)
                //.Column("CreatedBy", o.CreatedBy)
                //.Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return WeedConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// �ļ� ״̬ ����
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool WeedFileStatusUpdate(string id, bool status)
        {
            return Try(nameof(WeedFileStatusUpdate), () =>
            {
                var sql = @"update WeedFile set Status=@status where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id, status }).ToCommand();
                return WeedConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// �ļ� ɾ��
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool WeedFileDelete(string id)
        {
            return Try(nameof(WeedFileDelete), () =>
            {
                var sql = @"delete from WeedFile where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return WeedConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// �ļ� ��ȡ
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public WeedFile WeedFileGet(string id)
        {
            return Try(nameof(WeedFileGet), () =>
            {
                var sql = @"select * from WeedFile where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return WeedConn.QueryFirstOrDefault<WeedFile>(cmd);
            });
        }

        ///<summary>
        /// Ŀ¼ �Ƿ����
        ///</summary>
        ///<param name="uid">�û�id</param>
        ///<returns></returns>
        public int WeedDirExist(int uid)
        {
            return Try(nameof(WeedDirExist), () =>
            {
                var sql = @"select top 1 id from WeedDir where pid=0 and status=1 and depth=1 and uid=@uid";
                var cmd = SqlBuilder.Raw(sql, new { uid }).ToCommand();
                return WeedConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// �ļ� ��ҳ
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="did">Ŀ¼id</param>
        /// <param name="page">ҳ��</param>
        /// <param name="size">��ҳ��С</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="status">״̬</param>
        /// <returns></returns>
        public IPagedList<WeedFile> WeedFilePagedList(int uid, int did, int page = 1, int size = 20, string keyword = null, bool? status = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            return Try(nameof(WeedFilePagedList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("WeedFile")
                .Where(!string.IsNullOrEmpty(keyword), "Name=@keyword", new { keyword })
                .Where(status.HasValue, "Status=@status", new { status })
                .Where(uid > 0, "Uid=@uid", new { uid })
                .Where(did > 0, "Did=@did", new { did })
                .ToCommand(page, size);
                return WeedConn.PagedList<WeedFile>(page, size, cmd);
            });
        }

        #endregion
    }
}