using System;
using System.Runtime.Serialization;

namespace Exam.DAL.Exceptions
{
    /// <summary>
    /// استثناء يُستخدم عندما لا يتم العثور على كيان مطلوب في قاعدة البيانات
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        // اسم الكيان الذي لم يتم العثور عليه
        public string EntityName { get; }

        // معرف الكيان الذي لم يتم العثور عليه
        public object EntityId { get; }

        public NotFoundException() : base("الكيان المطلوب غير موجود")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string entityName, object entityId)
            : base($"لم يتم العثور على {entityName} بالمعرف {entityId}")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public NotFoundException(string entityName, object entityId, Exception innerException)
            : base($"لم يتم العثور على {entityName} بالمعرف {entityId}", innerException)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        // ضروري للتسلسل عبر التطبيقات أو النطاقات
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            EntityName = info.GetString(nameof(EntityName));
            EntityId = info.GetString(nameof(EntityId));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(EntityName), EntityName);
            info.AddValue(nameof(EntityId), EntityId);
        }
    }
}