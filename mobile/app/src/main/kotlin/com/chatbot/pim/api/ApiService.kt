package com.chatbot.pim.api

import com.chatbot.pim.api.models.LoginRequest
import com.chatbot.pim.api.models.LoginResponse
import com.chatbot.pim.api.models.RegisterRequest
import com.chatbot.pim.api.models.ChatRequest
import com.chatbot.pim.api.models.ChatResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface ApiService {
    @POST("/api/users/login")
    suspend fun login(@Body request: LoginRequest): Response<LoginResponse>

    @POST("/api/users/register")
    suspend fun register(@Body request: RegisterRequest): Response<LoginResponse>

    @POST("/api/chat")
    suspend fun sendMessage(@Body request: ChatRequest): Response<ChatResponse>
}
