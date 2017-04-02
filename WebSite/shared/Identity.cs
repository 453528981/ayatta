namespace Ayatta.Web
{
    /// <summary>
    /// 用户身份
    /// </summary>
    public sealed class Identity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }

        //public string Guid { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; }

        internal Identity(int id, string name = null, string displayName = null)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="identity"></param>
        public static implicit operator bool(Identity identity)
        {
            return identity.Id > 0 && !string.IsNullOrEmpty(identity.Name);
        }
    }
}
