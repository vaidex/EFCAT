namespace EFCAT.Model.Data.Extension {
    public static class ObjectExtension {
        public static bool IfNotNull<T>(this T obj, Predicate<T> predicate) {
            if (obj != null) return predicate(obj);
            return false;
        }
    }
}
