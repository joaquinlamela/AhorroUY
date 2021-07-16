package com.ahorrouy.ui.cart.adapter

import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import android.widget.ToggleButton
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.repository.model.CartProduct
import com.ahorrouy.ui.helpers.AdapterHelper
import com.squareup.picasso.Picasso

class CartAdapter(
    private val onRemoveClick: (CartProduct) -> Unit,
    private val onQualityChange: (CartProduct) -> Unit,
    private val onFavClick: (CartProduct, Boolean) -> Unit
) :
    ListAdapter<CartProduct, CartAdapter.CartViewHolder>(CartProduct.ProductDiffCallback) {

    inner class CartViewHolder(
        view: View,
        private val onRemoveClick: (CartProduct) -> Unit,
        private val onQualityChange: (CartProduct) -> Unit,
        private val onFavClick: (CartProduct, Boolean) -> Unit
    ) : RecyclerView.ViewHolder(view) {

        private val img: ImageView = view.findViewById(R.id.img_product)
        private val txtName: TextView = view.findViewById(R.id.txt_productName)
        private val txtDescription: TextView = view.findViewById(R.id.txt_market)
        private val txtPrice1: TextView = view.findViewById(R.id.txt_productPrice1)
        private val btnFavorite: ToggleButton = view.findViewById(R.id.btn_favorite)
        private val btnAdd: Button = view.findViewById(R.id.btn_plus)
        private val btnMinus: Button = view.findViewById(R.id.btn_minus)
        private val txtAmount: TextView = view.findViewById(R.id.txt_amount)

        private lateinit var currentItem: CartProduct
        private var index: Int = 0

        init {
            AdapterHelper.setUpFavStarAnimation(btnFavorite)
            btnFavorite.setOnClickListener {
                onFavClick(currentItem, btnFavorite.isChecked)
            }

            btnAdd.setOnClickListener {
                var amount = Integer.parseInt(txtAmount.text.toString())
                amount += 1
                txtAmount.text = amount.toString()
                currentItem.quantity = amount
                onQualityChange(currentItem)
            }
            btnMinus.setOnClickListener {
                var amount = Integer.parseInt(txtAmount.text.toString())
                if (amount == 1) {
                    onRemoveClick(currentItem)
                } else {
                    amount -= 1
                    txtAmount.text = amount.toString()
                    currentItem.quantity = amount
                    onQualityChange(currentItem)
                }
            }
        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(product: CartProduct, position: Int) {
            currentItem = product
            index = position
            Picasso.get().load(product.imageUrl).into(img)
            txtName.text = product.name
            txtDescription.text = product.description
            txtPrice1.text = "$${product.minPrice} - $${product.maxPrice}"
            btnFavorite.isChecked = product.isFavorite
            txtAmount.text = product.quantity.toString()
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): CartViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_cart, parent, false)
        return CartViewHolder(view, onRemoveClick, onQualityChange, onFavClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: CartViewHolder, position: Int) {
        val product = getItem(position)
        holder.bind(product, position)
    }
}