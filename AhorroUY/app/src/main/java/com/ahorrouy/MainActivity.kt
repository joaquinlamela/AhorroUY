package com.ahorrouy

import android.Manifest
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.os.Build
import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import androidx.recyclerview.widget.RecyclerView
import com.ahorrouy.databinding.ActivityMainBinding
import com.ahorrouy.repository.implementation.*
import com.ahorrouy.ui.cart.adapter.BottomCartAdapter
import com.ahorrouy.ui.cart.viewmodel.CartViewModel
import com.ahorrouy.ui.cart.viewmodel.CartViewModelFactory
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModel
import com.ahorrouy.ui.favorites.viewmodel.FavoritesViewModelFactory
import com.ahorrouy.ui.helpers.AdapterHelper
import com.ahorrouy.ui.home.viewmodel.HomeViewModel
import com.ahorrouy.ui.home.viewmodel.HomeViewModelFactory
import com.ahorrouy.ui.marketMap.model.ProductSearchModel
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.ahorrouy.ui.user.viewmodel.UserViewModelFactory
import com.google.android.material.bottomsheet.BottomSheetBehavior
import com.google.android.material.navigation.NavigationView
import com.vmadalin.easypermissions.EasyPermissions
import com.vmadalin.easypermissions.dialogs.SettingsDialog

class MainActivity : AppCompatActivity(), EasyPermissions.PermissionCallbacks {

    private lateinit var binding: ActivityMainBinding

    private val PERMISSIONS_REQUEST_ACCESS_FINE_LOCATION = 1
    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var drawerLayout: DrawerLayout
    private lateinit var homeVM: HomeViewModel
    private lateinit var userVM: UserViewModel
    private lateinit var favoritesVM: FavoritesViewModel
    private lateinit var cartVM: CartViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        val view = binding.root
        setContentView(view)

        // Create Favorites repository
        val favRepo = FavoritesRepository()

        // Create Shared ViewModel HomeViewModel
        val homeViewModelFactory = HomeViewModelFactory(
            CategoryRepository(), ProductRepository(), favRepo, TokenRepository()
        )
        homeVM =
            ViewModelProvider(this, homeViewModelFactory).get(HomeViewModel::class.java)

        // Create Shared ViewModel UserViewModel
        val userViewModelFactory = UserViewModelFactory(UserRepository())
        userVM =
            ViewModelProvider(this, userViewModelFactory).get(UserViewModel::class.java)

        // Create Shared ViewModel FavoritesViewModel
        val favoritesViewModelFactory = FavoritesViewModelFactory(favRepo)
        favoritesVM =
            ViewModelProvider(this, favoritesViewModelFactory).get(FavoritesViewModel::class.java)

        // Create Shared ViewModel CartViewModel
        val cartViewModelFactory = CartViewModelFactory(CartRepository())
        cartVM =
            ViewModelProvider(this, cartViewModelFactory).get(CartViewModel::class.java)

        // Set up toolbar
        val toolbar: Toolbar = findViewById(R.id.toolbar)
        setSupportActionBar(toolbar)
        createNotificationChannel()

        //Set up NavigationDrawer
        setUpNavigationDrawer()

        // On currentUser change (login or logout)
        userVM.currentUser.observe(this) { currentUser ->
            val loggedIn = currentUser != null
            setupMenu(loggedIn, currentUser)
            setupBottomSheetCart(loggedIn)
            if (loggedIn) favoritesVM.refresh(currentUser.Token)
            else {
                favoritesVM.clean()
                cartVM.clean()
            }
        }
        setupMenu(false, null)
        setupBottomSheetCart(false)
        handleFinishPurchaseButton(binding.root)

        setUpSession()
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    override fun onBackPressed() {
        if (drawerLayout.isDrawerOpen(GravityCompat.START)) {
            drawerLayout.closeDrawer(GravityCompat.START)
        } else {
            super.onBackPressed()
        }
    }

    private fun setUpNavigationDrawer() {
        drawerLayout = binding.drawerLayout
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.nav_home,
                R.id.nav_cart,
                R.id.nav_coupons,
                R.id.nav_favorites,
                R.id.nav_login,
                R.id.nav_logout,
                R.id.nav_signup,
            ), drawerLayout
        )
        val navView: NavigationView = findViewById(R.id.nav_view)
        val navController = findNavController(R.id.nav_host_fragment)
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        navView.menu.findItem(R.id.nav_logout).setOnMenuItemClickListener {
            userVM.logout()
            favoritesVM.clean()
            cartVM.clean()

            // goto Home
            navController.navigate(R.id.nav_home)
            val mDrawerLayout = findViewById<DrawerLayout>(R.id.drawer_layout)
            mDrawerLayout.closeDrawers()

            // Delete token from Internal Storage
            val fileName = "loginToken"
            openFileOutput(fileName, Context.MODE_PRIVATE).use { it?.write(0) }
            true
        }
    }

    private fun setupMenu(isLoggedIn: Boolean, user: UserViewModel.CurrentUser?) {
        val navView = findViewById<NavigationView>(R.id.nav_view)
        val menu = navView.menu
        val headerView: View = navView.getHeaderView(0)
        val txtMenuUsername = headerView.findViewById<TextView>(R.id.txt_menuUsername)
        menu.findItem(R.id.nav_cart).isVisible = isLoggedIn
        menu.findItem(R.id.nav_favorites).isVisible = isLoggedIn
        menu.findItem(R.id.nav_coupons).isVisible = isLoggedIn
        menu.findItem(R.id.nav_logout).isVisible = isLoggedIn
        menu.findItem(R.id.nav_login).isVisible = !isLoggedIn
        menu.findItem(R.id.nav_signup).isVisible = !isLoggedIn
        txtMenuUsername.text = "Visitante"
        if (isLoggedIn) {
            txtMenuUsername.text = user?.Username
        }
    }

    private fun setupBottomSheetCart(isLoggedIn: Boolean) {
        val bottomSheet = findViewById<ConstraintLayout>(R.id.bottom_sheet_persistent)
        if (!isLoggedIn) {
            bottomSheet.visibility = View.INVISIBLE
        } else {
            bottomSheet.setOnClickListener {
                val navController = findNavController(R.id.nav_host_fragment)
                navController.navigate(R.id.nav_cart)
            }
            val txtTotal = findViewById<TextView>(R.id.txt_total)
            val recyclerView = findViewById<RecyclerView>(R.id.recycler_view_bottom_cart)
            val bottomCartAdapter = BottomCartAdapter()
            recyclerView.adapter = bottomCartAdapter
            cartVM.cart.observe(this, Observer { cart ->
                if (cart.size > 0) {
                    bottomSheet.visibility = View.VISIBLE
                    var totalMin = 0.0
                    cart.forEach { totalMin += it.quantity * it.minPrice }
                    var totalMax = 0.0
                    cart.forEach { totalMax += it.quantity * it.maxPrice }
                    txtTotal.text = "$${totalMin} - $${totalMax}"
                    bottomCartAdapter.submitList(cart)
                    bottomCartAdapter.notifyDataSetChanged()
                    AdapterHelper.setUpOnAddCartAnimation(txtTotal)
                    val bottomSheetBehavior = BottomSheetBehavior.from(bottomSheet)
                    bottomSheetBehavior.state = BottomSheetBehavior.STATE_EXPANDED
                } else {
                    bottomSheet.visibility = View.INVISIBLE
                }
            })
        }
    }

    private fun createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val name = "channel1"
            val importance = NotificationManager.IMPORTANCE_HIGH
            val channel = NotificationChannel("channel1", name, importance)
            val notificationManager: NotificationManager =
                getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            notificationManager.createNotificationChannel(channel)
        }
    }

    private fun handleFinishPurchaseButton(root: View) {
        val btnFinishPurchase: Button = root.findViewById(R.id.btn_searchMarket)
        btnFinishPurchase.setOnClickListener {
            if (hasLocationPermission()) {
                goToMap()
            } else {
                requestLocationPermission()
            }
        }
    }

    private fun setUpSession() {
        try {
            openFileInput("currentUserToken").use { stream ->
                val token = stream?.bufferedReader().use {
                    it?.readText()
                }
                if (!token.isNullOrBlank() && token != "") {
                    openFileInput("currentUserName").use { stream ->
                        val username = stream?.bufferedReader().use {
                            it?.readText()
                        }
                        if (!username.isNullOrBlank() && username != "") {
                            userVM.login(token, username)
                            favoritesVM.refresh(token)
                        }
                    }
                }
            }
        } catch (ex: Exception) {
        }
    }

    private fun transformateProducts(): ArrayList<ProductSearchModel> {
        var listOfProductsToBuy: ArrayList<ProductSearchModel> = ArrayList<ProductSearchModel>()

        cartVM.cart.observe(this, Observer { cart ->
            cart.forEach { p ->
                var productSearchModel = ProductSearchModel(p.id, p.quantity)
                listOfProductsToBuy.add(productSearchModel)
            }
        })
        return listOfProductsToBuy
    }


    private fun hasLocationPermission() = EasyPermissions.hasPermissions(
        this,
        Manifest.permission.ACCESS_FINE_LOCATION
    )

    private fun requestLocationPermission() {
        EasyPermissions.requestPermissions(
            this,
            "Esta aplicacion requiere acceder a su ubicacion",
            PERMISSIONS_REQUEST_ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_FINE_LOCATION
        )
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        EasyPermissions.onRequestPermissionsResult(requestCode, permissions, grantResults, this)
    }

    override fun onPermissionsGranted(requestCode: Int, perms: List<String>) {
        when (requestCode) {
            PERMISSIONS_REQUEST_ACCESS_FINE_LOCATION -> {
                if (perms.isNotEmpty()) {
                    goToMap()
                }
            }
        }
    }

    override fun onPermissionsDenied(requestCode: Int, perms: List<String>) {
        if (EasyPermissions.somePermissionPermanentlyDenied(this, perms)) {
            SettingsDialog.Builder(this).build().show()
        } else {
            requestLocationPermission()
        }
    }

    private fun goToMap() {
        val bundle = Bundle()
        val listOfProductsToBuy = transformateProducts()
        bundle.putParcelableArrayList("List", listOfProductsToBuy)
        val navController = findNavController(R.id.nav_host_fragment)
        navController.navigate(R.id.nav_map, bundle)
    }
}