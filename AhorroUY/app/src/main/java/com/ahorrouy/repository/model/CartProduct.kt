package com.ahorrouy.repository.model

import androidx.recyclerview.widget.DiffUtil
import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.api.model.product.ProductResponse

class CartProduct(
    id: String,
    name: String,
    description: String,
    imageUrl: String,
    minPrice: Double,
    maxPrice: Double,
    isFavorite: Boolean,
    quantity: Int
) : AbstractProduct(id, name, description, imageUrl, minPrice, maxPrice, isFavorite, quantity) {
    companion object Factory {
        fun createFromProductResponse(p: ProductResponse, quantity: Int) = CartProduct(
            p.id,
            p.name,
            p.description,
            p.imageUrl,
            p.minPrice,
            p.maxPrice,
            false,
            quantity
        )

        fun createFromProductDiscountResponse(p: ProductDiscountResponse, quantity: Int) =
            CartProduct(
                p.id,
                p.name,
                p.description,
                p.imageUrl,
                p.minPrice,
                p.maxPrice,
                false,
                quantity
            )
    }

    object ProductDiffCallback : DiffUtil.ItemCallback<CartProduct>() {
        override fun areItemsTheSame(oldItem: CartProduct, newItem: CartProduct): Boolean =
            oldItem == newItem

        override fun areContentsTheSame(oldItem: CartProduct, newItem: CartProduct): Boolean =
            oldItem.id == newItem.id
    }
}
