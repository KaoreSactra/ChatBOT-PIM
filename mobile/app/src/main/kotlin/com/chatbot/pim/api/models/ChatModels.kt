package com.chatbot.pim.api.models

import com.squareup.moshi.Json
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class ChatMessage(
    @Json(name = "role")
    val role: String, // "user" ou "assistant"
    @Json(name = "content")
    val content: String
)

@JsonClass(generateAdapter = true)
data class ChatRequest(
    @Json(name = "messages")
    val messages: List<ChatMessage>
)

@JsonClass(generateAdapter = true)
data class ChatResponse(
    @Json(name = "success")
    val success: Boolean,
    @Json(name = "message")
    val message: String,
    @Json(name = "response")
    val response: String
)
