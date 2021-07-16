package com.ahorrouy.ui.marketMap.adapter

import android.annotation.SuppressLint
import android.content.Context
import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.ImageView
import android.widget.TextView
import com.ahorrouy.R
import com.ahorrouy.api.model.marketMap.BestOptionResponse
import com.squareup.picasso.Picasso


class MyAdapter(private val context: Context, private val arrayList: List<BestOptionResponse>) :
    BaseAdapter() {
    private lateinit var marketName: TextView
    private lateinit var price: TextView
    private lateinit var marketAddress: TextView
    private lateinit var marketLogo: ImageView
    private lateinit var marketTimeToClose: TextView

    override fun getCount(): Int {
        return arrayList.size
    }

    override fun getItem(position: Int): Any {
        return position
    }

    override fun getItemId(position: Int): Long {
        return position.toLong()
    }

    @SuppressLint("ResourceType")
    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View? {
        var convertView = convertView
        convertView = LayoutInflater.from(context).inflate(R.layout.list_item, parent, false)
        marketName = convertView.findViewById(R.id.marketName)
        price = convertView.findViewById(R.id.marketPrice)
        marketAddress = convertView.findViewById(R.id.marketAddress)
        marketLogo = convertView.findViewById(R.id.marketLogo)
        marketTimeToClose = convertView.findViewById(R.id.marketTimeToClose)
        Picasso.get().load(arrayList[position].marketLogo).into(marketLogo)
        marketName.text = arrayList[position].marketName
        price.text = "\$ ${arrayList[position].priceForProducts}"
        marketAddress.text = arrayList[position].marketAddress
        marketTimeToClose.text = "Cierra en: ${arrayList[position].difference.toInt()} horas"
        if (position == 0) {
            convertView.setBackgroundColor(
                Color.rgb(255, 255, 204)
            )
        }
        return convertView
    }
}