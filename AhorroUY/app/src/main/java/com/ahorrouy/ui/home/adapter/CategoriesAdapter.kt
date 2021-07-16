package com.ahorrouy.ui.home.adapter

import android.os.Build
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.annotation.RequiresApi
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.R
import com.ahorrouy.api.model.category.CategoryResponse
import com.squareup.picasso.Picasso

class CategoriesAdapter(private val onClick: (CategoryResponse) -> Unit) :
    ListAdapter<CategoryResponse, CategoriesAdapter.CategoriesViewHolder>(CategoryDiffCallback) {

    inner class CategoriesViewHolder(view: View, val onClick: (CategoryResponse) -> Unit) :
        RecyclerView.ViewHolder(view) {

        private val imageViewCategory: ImageView = view.findViewById(R.id.img_category)
        private val textViewCategoryName: TextView = view.findViewById(R.id.txt_categoryName)

        private lateinit var currentItem: CategoryResponse

        init {
            imageViewCategory.setOnClickListener { onClick(currentItem) }
        }

        @RequiresApi(Build.VERSION_CODES.O)
        fun bind(category: CategoryResponse) {
            currentItem = category
            Picasso.get().load(category.imageUrl).into(imageViewCategory)
            textViewCategoryName.text = category.categoryName
        }
    }

    override fun onCreateViewHolder(
        parent: ViewGroup,
        viewType: Int
    ): CategoriesAdapter.CategoriesViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_category, parent, false)
        return CategoriesViewHolder(view, onClick)
    }

    @RequiresApi(Build.VERSION_CODES.O)
    override fun onBindViewHolder(holder: CategoriesAdapter.CategoriesViewHolder, position: Int) {
        val cat = getItem(position)
        holder.bind(cat)
    }
}

object CategoryDiffCallback : DiffUtil.ItemCallback<CategoryResponse>() {
    override fun areItemsTheSame(oldItem: CategoryResponse, newItem: CategoryResponse): Boolean =
        oldItem == newItem

    override fun areContentsTheSame(oldItem: CategoryResponse, newItem: CategoryResponse): Boolean =
        oldItem.id == newItem.id
}