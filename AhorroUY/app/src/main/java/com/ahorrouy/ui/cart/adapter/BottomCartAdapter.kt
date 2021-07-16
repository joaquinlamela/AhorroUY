package com.ahorrouy.ui.cart.adapter

import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.repository.model.CartProduct

class BottomCartAdapter :
    ListAdapter<CartProduct, BottomCartAdapter.CartViewHolder>(CartProduct.ProductDiffCallback) {

    inner class CartViewHolder(view: View) : RecyclerView.ViewHolder(view) {

        private val txtName: TextView = view.findViewById(R.id.txt_productName)
        private val txtPrice1: TextView = view.findViewById(R.id.txt_productPrice1)
        private val txtAmount: TextView = view.findViewById(R.id.txt_amount)

        private lateinit var currentItem: CartProduct

        init {
        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(product: CartProduct, position: Int) {
            currentItem = product
            txtName.text = product.name
            txtPrice1.text = "$${product.minPrice} - $${product.maxPrice}"
            txtAmount.text = product.quantity.toString()
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CartViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_bottom_cart, parent, false)
        return CartViewHolder(view)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: CartViewHolder, position: Int) {
        val product = getItem(position)
        holder.bind(product, position)
    }
}