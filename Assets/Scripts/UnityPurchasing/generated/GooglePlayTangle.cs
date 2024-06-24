#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ievJz7udnz3ysOWpE7X1ZYXxxPU6/ruELzXj3h1hUj3wXTXtYCSJbnYC1YiWlWfNxEsIIyKZ4fzghFr2C6L4xnF/a6euWKDuI5XXtWd5SpQV5+4GXVv7oHWzDrGmWEfKUzNvf37MT2x+Q0hHZMgGyLlDT09PS05Nae0E7h8AafyUvlOdsk7/qIdmy38eIWlDTU6w3+FcEWfb5BQtRKhXuWLnOniGRDq0qzue3fngEMaSSv4cjP8vEyEQIf1k17Xv3L+PPnf7v6rMT0FOfsxPREzMT09O3Ggaokppq02fS39AJqr43Y8vlqv8MDvZRDErcG/7zSuWm69yJSKa0gT9JQuSAziUJJNBAhOjj+xXXi8DyM2ywDU5gYsuQH70mhuw1UxNT05P");
        private static int[] order = new int[] { 4,6,10,9,12,12,9,10,10,11,11,12,13,13,14 };
        private static int key = 78;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
