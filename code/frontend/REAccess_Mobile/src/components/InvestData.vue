<template>
  <div class="filter-data">
      <div v-for="(item,index) in filterList" :key="String(index)" class="line-box" @click="goToDetail(item)">
        <div class="line-item num-order" v-if="item.rankPlace===1">
            <img src="@/assets/1.svg" class="order-img"/>
        </div>
        <div class="line-item num-order" v-if="item.rankPlace===2">
            <img src="@/assets/2.svg" class="order-img"/>
        </div>
         <div class="line-item num-order" v-if="item.rankPlace===3">
            <img src="@/assets/3.svg" class="order-img"/>
        </div>
        <div class="line-item num-order" v-else-if="item.rankPlace>3">#{{item.rankPlace}}</div>
        <template v-if="searchName==='所在区域'">
          <div :class="index>2?'line-item uni-font':'line-item'" style="color:#333333">{{item.cityName}}</div>
          <div class="line-item uni-font">{{item.provinceName}}</div>
          <div class="line-item uni-font amount-box">{{item.rankValue}}{{item.unit}}</div>
        </template>
        <template v-else>
          <div class="line-item content-item">{{item.indexName}}</div>
          <div class="line-item uni-font amount-box search-else">{{item.indexValue}}{{item.unit}}</div>
        </template>
        <div class="line-item goto-order"><img src="@/assets/next.svg"/></div>
      </div>
  </div>
</template>

<script>
export default {
  name: 'FilterIndex',
  props:{
   filterList: Array,
   searchName: String,
   tabName:String
  },
  data () {
    return {
     
    }
  },
  created(){
   
  },
  computed:{
  
  },
  methods:{
    goToDetail(item){
      console.log(item)
      const id = item.districtSk?item.districtSk:item.indexId
      if(this.tabName === 'list'){
         this.$router.push(`/IndustrialInvest/ListDetail?searchName=${this.searchName}&id=${id}`)
      }else{
        this.$router.push(`/IndustrialInvest/LandDetail?searchName=${this.searchName}&id=${id}`)
      }
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.filter-title{
  background-image: linear-gradient(-85deg, #FFFFFF 0%, #E6EFFF 98%);
  border-radius: .25rem .25rem 0 0;
  height: 3rem;
  line-height: 3rem;
  text-align: left;
  padding: 0 1.5rem;
}
.logo-box{
  float: left;
  margin-top: 0.8rem;
  margin-right: 1rem;
}
.search-title{
  font-size: 1rem;
  color: #000000;
  float: left;
  max-width: 50%;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  font-weight: 700;
}
.unit-box{
  background: #E3EBFF;
  border-radius: 4px;
  float: right;
  height: 1.25rem;
  line-height: 1.25rem;
  padding: 0 0.5rem;
  font-size: .625rem;
  color: #666666;
  margin-top: 0.8rem;
  margin-right: 1rem;
}
.unit-box::before{
    position: absolute;
    content: '';
    width: 0;
    height: 0;
    border-width: 6px 3px;
    border-style: solid;
    border-color: #E3EBFF transparent transparent;
    margin-left: -0.5rem;
    margin-top: 1.125rem;
}
.line-box{
  height: 4rem;
  line-height: 4rem;
  font-size: .75rem;
  color: #333333;
  margin: 0 1.5rem 0 1.5rem;
  border-bottom: 1px solid #EEEFF3;
}
.num-order{
  width: 10% !important;
}
.goto-order{
  width: 10% !important;
  text-align: right !important;
}
.goto-order >>> img{
    width: 0.7rem;
    margin-top: 1.6rem;
    float: right;
}
.line-item{
  float: left;
  width: 26%;
  text-align: left;
}
.content-item{
  width: 58% !important;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
.search-else{
  width: 20% !important;
}
.uni-font{
 color: #666666;
}
.order-img{
  float: left;
  margin-top: 1.25rem;
  margin-left: -0.1875rem;
}
.amount-box{
  margin-left: 0%;
  text-align: center;
}
.filter-data{
  min-height: 20.5rem;
}
</style>
