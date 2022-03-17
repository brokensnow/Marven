<template>
  <div class="city-detail">
   <div :class="chooseItem==='list'?'shift-tab active-tab':'shift-tab'" @click="shiftTag('list')">
     <img src="@/assets/上市.svg" class="tab-img"/><span>A股上市企业投资</span>
     <div class="year-box" v-if="chooseItem==='list'">2021年</div>
   </div>
   <div :class="chooseItem==='land'?'shift-tab active-tab':'shift-tab'"  @click="shiftTag('land')">
     <img src="@/assets/产业用地.svg" class="tab-img"/><span>产业用地获取</span>      
     <div class="year-box" v-if="chooseItem==='land'">2021年 
       <!-- <a class="el-icon-arrow-down"></a> -->
       </div>   
   </div>
     <invest-filter-index ref="investFilterIndex" :title="chooseOption" :filterList="options" @searchData="searchData"></invest-filter-index>
     <invest-data :filterList="filterList" :searchName="chooseOption" :tabName="chooseItem"></invest-data>
     <div class="footer">
        <div class="footer-img">
           <!-- <img src="@/assets/解锁更多投资项目.svg"/> -->
            <div class="footer-img-bg">
             <div class="footer-line-box">
              <img src="@/assets/解锁.svg" class="footer-icon-size"/>
              <span class="footer-img-font">解锁更多投资项目</span>
             </div>
             <div class="footer-line-box1">
               <img src="@/assets/登录.svg" class="footer-icon-size"/>
               <span class="footer-img-font2">请登录PC端 (rea.deloitte.com.cn)</span>
             </div>
            </div>
        </div>
     </div>
  </div>
</template>

<script>
import api from "@/request/api";
import { mapState } from 'vuex'
import InvestFilterIndex from "../../components/InvestFilterIndex.vue";
import InvestData from "../../components/InvestData.vue";
export default {
  name: 'IndustrialInvest',
  components:{
    InvestFilterIndex,
    InvestData
  },
  computed: {
    ...mapState('common/common', ['searchName','routerName']),
  },
  data(){
    return{
       chooseItem:"list",
       chooseOption:"所在区域",
       options: ['所在区域','所属产业'],
       filterList: []
    }
  },
  created(){
    // const searchName = window.localStorage.getItem('searchName')
    // const routerName = window.localStorage.getItem('routerName')
    const searchName = this.searchName
    const routerName = this.routerName
    if(routerName === 'LandDetail'){
        this.chooseItem = 'land'
    }
    if(searchName === '所属产业'){
        this.chooseOption = '所属产业'
    }
    this.getInvestList()
    this.$store.commit('common/common/setSearchName', '')
    this.$store.commit('common/common/setRouterName', '')
    // window.localStorage.setItem('searchName', '')
    // window.localStorage.setItem('routerName', '')
  },
  methods:{
    shiftTag(value){
      this.chooseItem = value
      this.getInvestList()
      this.$refs.investFilterIndex.chooseIndex('所在区域')
    },
    searchData(value){
      this.chooseOption = value
      this.getInvestList()
    },
    getInvestList(){
      if(this.chooseItem === 'list'){
        if(this.chooseOption === '所在区域'){
            api
            .get('/Industry/GetActiveAreaRank')
            .then((res) => {
              this.filterList = res.data.returnObj
            });
        }else{
            api
            .get('/Industry/GetHotIndustryRank')
            .then((res) => {
              this.filterList = res.data.returnObj
            });
        }
      }else{
        if(this.chooseOption === '所在区域'){
            api
            .get('/Industry/GetIndusrtyActiveRegion')
            .then((res) => {
              this.filterList = res.data.returnObj
            });
        }else{
            api
            .get('/Industry/GetLandHotIndustryRank')
            .then((res) => {
              this.filterList = res.data.returnObj
            });
        }
      }
    },
  }
}
</script>

<style scoped>
.city-detail{
  background: #fff;
  width: 100%;
  min-height: 79.8vh;
  border-top: 1px solid #f0f1f4;
}
.shift-tab{
  background: #D5D5D5;
  border-radius: 4px 4px 0 0;
  width: 50%;
  float: left;
  height: 3.5rem;
  line-height: 3.5rem;
  font-size: .875rem;
  color: #666666;
  margin-bottom: 1rem;
}
.shift-tab >>> span{
  margin-left: 1.2rem;
}
.tab-img {
    position: absolute;
    margin-top: 1.3rem;
}
.active-tab{
  color: #333333;
  background: #FFFFFF;
}
.footer{
 text-align: center;
 width: 100%;
 /* position: fixed; */
 bottom: 0;
 background: #fff;
 margin-top: 2rem;
 /* padding: 1rem; */
}
.footer-img >>> img{
  /* width: 94%; */
}
.footer-img{
  /* text-align: center; */
}
.year-box{
  font-size: 0.75rem;
  color: #666666;
  margin-top: -2rem;
}
</style>
