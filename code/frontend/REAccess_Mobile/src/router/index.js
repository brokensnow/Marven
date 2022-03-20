import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: () => import('../views/home/Index.vue'),
      meta: {
        title: '德勤 | 产城智链'
      }
    },
    {
      path: '/Detail',
      name: 'Detail',
      component: () => import('../views/home/Detail.vue'),
      meta: {
        title: '德勤 | 产城智链 | 资讯'
      }
    },
    {
      path: '/SingleIndex',
      name: 'SingleIndex',
      component: () => import('../views/singleIndex/Index.vue'),
      meta: {
        title: '德勤 | 产城智链 | 单项指标'
      }
    },
    {
      path: '/CityRanking',
      name: 'CityRanking',
      component: () => import('../views/cityRanking/Index.vue'),
      meta: {
        title: '德勤 | 产城智链 | 城市排名'
      }
    },
    {
    path: '/IndustrialInvest',
      name: 'IndustrialInvest',
      component: () => import('../views/industrialInvest/Index.vue'),
      meta: {
        title: '德勤 | 产城智链 | 产业投资'
      }
    },
    {
      path: '/SpecialPolicies',
      name: 'SpecialPolicies',
      component: () => import('../views/specialPolicies/Index.vue'),
      meta: {
        title: '德勤 | 产城智链 | 专项政策'
      }
    },
    {
      path: '/Agreement',
      name: 'Agreement',
      component: () => import('../views/standard/Agreement.vue'),
      meta: {
        title: '德勤 | 产城智链 | 试用协议'
      }
    },
    {
      path: '/Contact',
      name: 'Contact',
      component: () => import('../views/standard/Contact.vue'),
      meta: {
        title: '德勤 | 产城智链 | 联系我们'
      }
    }, 
    {
      path: '/PrivacyPolicy',
      name: 'PrivacyPolicy',
      component: () => import('../views/standard/PrivacyPolicy.vue'),
      meta: {
        title: '德勤 | 产城智链 | 隐私政策'
      }
    },
    {
      path: '/IndustrialInvest/ListDetail',
      name: 'ListDetail',
      component: () => import('../views/industrialInvest/components/ListDetail.vue'),
      meta: {
        title: '德勤 | 产城智链 | 产业投资'
      }
    },
    {
      path: '/IndustrialInvest/LandDetail',
      name: 'LandDetail',
      component: () => import('../views/industrialInvest/components/LandDetail.vue'),
      meta: {
        title: '德勤 | 产城智链 | 产业投资'
      }
    },
    {
      path: '/PolicyDetail',
      name: 'PolicyDetail',
      component: () => import('../views/specialPolicies/components/PolicyDetail.vue'),
      meta: {
        title: '德勤 | 产城智链 | 专项政策'
      }
    },
  ]
})
