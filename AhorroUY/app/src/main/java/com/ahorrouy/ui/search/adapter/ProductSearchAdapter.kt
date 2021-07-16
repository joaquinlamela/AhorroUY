package com.ahorrouy.ui.search.adapter

import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.View.INVISIBLE
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
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.ui.helpers.AdapterHelper
import com.squareup.picasso.Picasso

class ProductSearchAdapter(
    private val isLogged: Boolean,
    private val onAddCartClick: (View, ProductResponse, Int) -> Unit,
    private val onFavClick: (ProductResponse, Boolean) -> Unit
) : ListAdapter<ProductResponse, ProductSearchAdapter.SearchProductHolder>(ProductsDiffCallback2) {

    inner class SearchProductHolder(
        val view: View,
        private val isLogged: Boolean,
        private val onAddCartClick: (View, ProductResponse, Int) -> Unit,
        private val onFavClick: (ProductResponse, Boolean) -> Unit
    ) : RecyclerView.ViewHolder(view) {

        private val img: ImageView = view.findViewById(R.id.img_product)
        private val txtName: TextView = view.findViewById(R.id.txt_productName)
        private val txtDescription: TextView = view.findViewById(R.id.txt_market)
        private val txtPrice1: TextView = view.findViewById(R.id.txt_productPrice1)
        private val btnFavorite: ToggleButton = view.findViewById(R.id.btn_favorite)
        private val btnAdd: Button = view.findViewById(R.id.btn_plus)
        private val btnMinus: Button = view.findViewById(R.id.btn_minus)
        private val txtAmount: TextView = view.findViewById(R.id.txt_amount)
        private val btnAddCart: Button = view.findViewById(R.id.btn_addCart)

        private lateinit var currentItem: ProductResponse

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
        fun bind(product: ProductResponse) {
            currentItem = product
            txtName.text = product.name
            txtDescription.text = product.description
            txtPrice1.text = "$ ${product.minPrice} - $ ${product.maxPrice}"
            Picasso.get().load(product.imageUrl).into(img)
            if (isLogged) {
                btnFavorite.isChecked = product.isFavorite
            } else {
                btnFavorite.visibility = INVISIBLE
            }
        }
    }

    override fun onCreateViewHolder(
        parent: ViewGroup,
        viewType: Int
    ): ProductSearchAdapter.SearchProductHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_product, parent, false)
        return SearchProductHolder(view, isLogged, onAddCartClick, onFavClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(
        holder: ProductSearchAdapter.SearchProductHolder,
        position: Int
    ) {
        holder.bind(getItem(position))
    }

}

object ProductsDiffCallback2 : DiffUtil.ItemCallback<ProductResponse>() {
    override fun areItemsTheSame(oldItem: ProductResponse, newItem: ProductResponse): Boolean =
        oldItem == newItem

    override fun areContentsTheSame(oldItem: ProductResponse, newItem: ProductResponse): Boolean =
        oldItem.id == newItem.id
}