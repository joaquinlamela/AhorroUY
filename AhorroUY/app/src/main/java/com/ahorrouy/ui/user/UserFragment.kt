package com.ahorrouy.ui.user

import android.app.Activity
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.widget.Toolbar
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.viewpager2.adapter.FragmentStateAdapter
import androidx.viewpager2.widget.ViewPager2
import com.ahorrouy.R
import com.ahorrouy.repository.implementation.UserRepository
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.ahorrouy.ui.user.viewmodel.UserViewModelFactory
import com.google.android.material.tabs.TabLayout
import com.google.android.material.tabs.TabLayout.OnTabSelectedListener
import com.google.android.material.tabs.TabLayoutMediator

class UserFragment : Fragment() {

    private lateinit var userViewModel: UserViewModel

    private lateinit var tabsAdapter: TabsAdapter
    private lateinit var tabLayout: TabLayout
    private lateinit var viewPager: ViewPager2

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val repository = UserRepository()
        val viewModelFactory = UserViewModelFactory(repository)
        userViewModel =
            ViewModelProvider(this, viewModelFactory).get(UserViewModel::class.java)
        return inflater.inflate(R.layout.fragment_user, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        tabsAdapter = TabsAdapter(this)
        viewPager = view.findViewById(R.id.view_pager)
        viewPager.adapter = tabsAdapter

        tabLayout = view.findViewById(R.id.tabs)
        setTabMediator()
        onTabSelected()
        setContent()
    }

    private fun setTabMediator() {
        TabLayoutMediator(tabLayout, viewPager) { tab, position ->
            when (position) {
                0 -> {
                    tab.text = "Registrarse"
                }
                1 -> {
                    tab.text = "Iniciar sesión"
                }
            }
        }.attach()
    }

    private fun onTabSelected() {
        tabLayout.addOnTabSelectedListener(object : OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab) {
                val toolbar = (context as Activity).findViewById<Toolbar>(R.id.toolbar)
                if (tabLayout.selectedTabPosition == 0)
                    toolbar?.title = "Registrarse"
                else
                    toolbar?.title = "Iniciar sesión"
            }

            override fun onTabUnselected(tab: TabLayout.Tab?) {}
            override fun onTabReselected(tab: TabLayout.Tab?) {}
        })
    }

    private fun setContent() {
        val index = arguments?.getInt("index")
        tabsAdapter.notifyDataSetChanged()
        viewPager.setCurrentItem(index as Int, false)
        val tab = tabLayout.getTabAt(index)
        tab!!.select()
    }

}

class TabsAdapter(fragment: Fragment) : FragmentStateAdapter(fragment) {
    override fun getItemCount(): Int = 2
    override fun createFragment(position: Int): Fragment {
        return if (position == 0) {
            SignupFragment()
        } else {
            LoginFragment()
        }
    }
}