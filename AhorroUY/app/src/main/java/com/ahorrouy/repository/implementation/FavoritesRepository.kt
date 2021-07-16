package com.ahorrouy.repository.implementation

import android.util.Log
import com.ahorrouy.api.RetrofitInstance
import com.ahorrouy.api.model.product.ProductResponse

class FavoritesRepository {

    private var favorites: MutableList<ProductResponse> = mutableListOf()

    suspend fun refresh(auth: String): MutableList<ProductResponse> {
        try {
            val response = RetrofitInstance.favoritesEndpoints.getFavorites(auth)
            if (response.isSuccessful) {
                favorites = response.body() as MutableList<ProductResponse>
            }
        } catch (ex: Exception) {
            Log.e("Exception on refresh favorites", ex.message!!)
        }
        return favorites
    }

    fun getFavorites(): MutableList<ProductResponse> {
        return favorites
    }

    suspend fun add(auth: String, productId: String) {
        try {
            val response = RetrofitInstance.favoritesEndpoints.postFavorites(auth, productId)
            if (response.isSuccessful) {
                val product = response.body() as ProductResponse
                favorites.add(product)
            }
        } catch (ex: Exception) {
            Log.e("Exception on add favorites", ex.message!!)
        }
    }

    suspend fun delete(auth: String, productId: String) {
        try {
            RetrofitInstance.favoritesEndpoints.deleteFavorites(auth, productId)
            val index = favorites.indexOfFirst { it.id == productId }
            favorites.removeAt(index)
        } catch (ex: Exception) {
            Log.e("Exception on delete favorites", ex.message!!)
        }
    }

    fun clean() {
        favorites.clear()
    }
}