package com.ahorrouy.repository.implementation

import com.ahorrouy.repository.model.CartProduct

class CartRepository {

    private val products: MutableList<CartProduct> = mutableListOf()

    fun getCart(): MutableList<CartProduct> {
        return products
    }

    fun addProduct(product: CartProduct) {
        val existingProduct = products.find { p -> p.id == product.id }
        if (existingProduct != null) {
            existingProduct.quantity = product.quantity
        } else {
            products.add(product)
        }
    }

    fun removeProduct(product: CartProduct) {
        products.remove(product)
    }

    fun newQuantity(product: CartProduct) {
        val p = products.find { p -> p.id == product.id }
        if (p != null) p.quantity = product.quantity
    }

    fun clean() {
        products.removeAll(products)
    }

}

