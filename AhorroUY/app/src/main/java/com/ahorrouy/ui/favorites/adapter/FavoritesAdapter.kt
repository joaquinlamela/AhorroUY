package com.ahorrouy.ui.favorites.adapter

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
import com.ahorrouy.api.model.product.ProductResponse
import com.ahorrouy.ui.helpers.AdapterHelper
import com.squareup.picasso.Picasso

class FavoritesAdapter(
    private val onFavClick: (ProductResponse) -> Unit,
    private val onAddCartClick: (View, ProductResponse, Int) -> Unit
) : ListAdapter<ProductResponse, FavoritesAdapter.FavoritesViewHolder>(FavoriteDiffCallback) {

    inner class FavoritesViewHolder(
        view: View,
        val onFavClick: (ProductResponse) -> Unit,
        val onAddCartClick: (View, ProductResponse, Int) -> Unit
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
            AdapterHelper.setUpFavStarAnimation(btnFavorite)
            btnFavorite.setOnClickListener {
                onFavClick(currentItem)
            }

            AdapterHelper.setUpPlusMinusButtons(btnAdd, btnMinus, txtAmount)

            btnAddCart.setOnClickListener {
                onAddCartClick(view, currentItem, Integer.parseInt(txtAmount.text.toString()))
            }

        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(favorite: ProductResponse) {
            currentItem = favorite
            Picasso.get().load(favorite.imageUrl).into(img)
            btnFavorite.isChecked = true
            txtName.text = favorite.name
            txtDescription.text = favorite.description
            txtPrice1.text = "$${favorite.minPrice} - $${favorite.maxPrice}"
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): FavoritesViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_product, parent, false)
        return FavoritesViewHolder(view, onFavClick, onAddCartClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: FavoritesViewHolder, position: Int) {
        val favorite = getItem(position)
        holder.bind(favorite)
    }
}

object FavoriteDiffCallback : DiffUtil.ItemCallback<ProductResponse>() {
    override fun areItemsTheSame(oldItem: ProductResponse, newItem: ProductResponse): Boolean =
        oldItem == newItem

    override fun areContentsTheSame(oldItem: ProductResponse, newItem: ProductResponse): Boolean =
        oldItem.id == newItem.id
}