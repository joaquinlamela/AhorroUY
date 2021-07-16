package com.ahorrouy.ui.home.adapter

import android.graphics.Paint
import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import android.widget.ToggleButton
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.product.ProductDiscountResponse
import com.ahorrouy.ui.helpers.AdapterHelper
import com.squareup.picasso.Picasso

class ProductDiscountAdapter(
    private val isLogged: Boolean,
    private val onAddCartClick: (View, ProductDiscountResponse, Int) -> Unit,
    private val onFavClick: (ProductDiscountResponse, Boolean) -> Unit
) : ListAdapter<ProductDiscountResponse, ProductDiscountAdapter.ProductDiscountHolder>(
    ProductsDiffCallback
) {

    inner class ProductDiscountHolder(
        val view: View,
        private val isLogged: Boolean,
        private val onAddCartClick: (View, ProductDiscountResponse, Int) -> Unit,
        private val onFavClick: (ProductDiscountResponse, Boolean) -> Unit
    ) : RecyclerView.ViewHolder(view) {

        private val imgProduct: ImageView = view.findViewById(R.id.img_product)
        private val txtName: TextView = view.findViewById(R.id.txt_productName)
        private val txtMarket: TextView = view.findViewById(R.id.txt_market)
        private val txtPrice1: TextView = view.findViewById(R.id.txt_productPrice1)
        private val txtPrice2: TextView = view.findViewById(R.id.txt_productPrice2)
        private val btnFavorite: ToggleButton = view.findViewById(R.id.btn_favorite)
        private val btnAdd: Button = view.findViewById(R.id.btn_plus)
        private val btnMinus: Button = view.findViewById(R.id.btn_minus)
        private val txtAmount: TextView = view.findViewById(R.id.txt_amount)
        private val btnAddCart: Button = view.findViewById(R.id.btn_addCart)
        private val txtDiscount: TextView = view.findViewById(R.id.txt_discount)

        private lateinit var currentItem: ProductDiscountResponse

        init {
            if (isLogged) {
                AdapterHelper.setUpFavStarAnimation(btnFavorite)
                btnFavorite.setOnClickListener {
                    onFavClick(currentItem, btnFavorite.isChecked)
                }
            }

            AdapterHelper.setUpPlusMinusButtons(btnAdd, btnMinus, txtAmount)

            btnAddCart.setOnClickListener {
                onAddCartClick(view, currentItem, Integer.parseInt(txtAmount.text.toString()))
            }

        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(productDiscount: ProductDiscountResponse) {
            currentItem = productDiscount
            txtName.text = productDiscount.name
            txtMarket.text = productDiscount.marketName
            txtPrice1.apply {
                paintFlags = paintFlags or Paint.STRIKE_THRU_TEXT_FLAG
                text = "$${productDiscount.regularPrice}"
            }
            txtPrice2.text = "$${productDiscount.currentPrice}"
            txtDiscount.text = "-${productDiscount.discountPercentage.toInt()}%"
            Picasso.get().load(productDiscount.imageUrl).into(imgProduct)
            if (isLogged) {
                btnFavorite.isChecked = productDiscount.isFavorite
            } else {
                btnFavorite.visibility = View.INVISIBLE
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int):
            ProductDiscountAdapter.ProductDiscountHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_product_discount, parent, false)
        return ProductDiscountHolder(view, isLogged, onAddCartClick, onFavClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(
        holder: ProductDiscountAdapter.ProductDiscountHolder,
        position: Int
    ) {
        holder.bind(getItem(position))
    }

}

object ProductsDiffCallback : DiffUtil.ItemCallback<ProductDiscountResponse>() {
    override fun areItemsTheSame(
        oldItem: ProductDiscountResponse,
        newItem: ProductDiscountResponse
    ): Boolean = oldItem == newItem

    override fun areContentsTheSame(
        oldItem: ProductDiscountResponse,
        newItem: ProductDiscountResponse
    ): Boolean = oldItem.name == newItem.name
}