# Retrofit
-keep class com.squareup.retrofit2.** { *; }
-keepattributes Signature
-keepattributes *Annotation*

# OkHttp
-keep class okhttp3.** { *; }
-keep interface okhttp3.** { *; }
-dontwarn okhttp3.**

# Moshi
-keep class com.squareup.moshi.** { *; }
-keep interface com.squareup.moshi.** { *; }
-keep class com.chatbot.pim.api.models.** { *; }
-keepclasseswithmembers class com.chatbot.pim.api.models.** {
    <init>(...);
}

# Kotlin
-keep class kotlin.** { *; }
-dontwarn kotlin.**

# Coroutines
-keep class kotlinx.coroutines.** { *; }
-dontwarn kotlinx.coroutines.**

# Compose
-keep class androidx.compose.** { *; }
-dontwarn androidx.compose.**

# App code
-keepclasseswithmembernames class com.chatbot.pim.** {
    native <methods>;
}

-keep public class * extends androidx.lifecycle.ViewModel
