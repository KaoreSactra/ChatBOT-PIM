package com.chatbot.pim.api.models

import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class LoginRequest(
    @Json(name = "email")
    val email: String,
    @Json(name = "password")
    val password: String
)

@JsonClass(generateAdapter = true)
data class RegisterRequest(
    @Json(name = "email")
    val email: String,
    @Json(name = "password")
    val password: String,
    @Json(name = "role")
    val role: String = "user"
)

@JsonClass(generateAdapter = true)
data class LoginResponse(
    @Json(name = "success")
    val success: Boolean,
    @Json(name = "message")
    val message: String,
    @Json(name = "user")
    val user: UserResponse?
)

@JsonClass(generateAdapter = true)
data class UserResponse(
    @Json(name = "id")
    val id: Int,
    @Json(name = "email")
    val email: String,
    @Json(name = "role")
    val role: String
)
