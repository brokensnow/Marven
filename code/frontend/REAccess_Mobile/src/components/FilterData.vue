<template>
  <div class="filter-data">
      <div class="filter-title"><img src="@/assets/城市排名.svg" class="logo-box"/>
        <div class="search-title">{{searchName}}</div>
        <!-- <div class="unit-box">单位：{{rankingData.unit}}</div> -->
        <div class="unit-box">{{rankingData.year}}年 排名</div>
      </div>
      <div v-for="(item,index) in rankingData.singleIndexList" :key="String(index)" class="line-box">
        <div class="line-item num-order" v-if="index===0">
            <img src="@/assets/1.svg" class="order-img"/>
        </div>
        <div class="line-item num-order" v-if="index===1">
            <img src="@/assets/2.svg" class="order-img"/>
        </div>
         <div class="line-item num-order" v-if="index===2">
            <img src="@/assets/3.svg" class="order-img"/>
        </div>
        <div class="line-item num-order" v-if="index>2">#{{index+1}}</div>
        <div :class="index>2?'line-item uni-font':'line-item'" style="line-height: 1rem;margin-top: 1.5rem;padding-right:4%;width:26%;color:#333333">{{item.cityName}}</div>
        <div class="line-item uni-font" style="line-height: 1rem;margin-top: 1.5rem;padding-right:3%;width:28%">{{item.provinceName}}</div>
        <div class="line-item uni-font amount-box">{{item.rankValue}} {{item.unit}}</div>
      </div>
  </div>
</template>

<script>
export default {
  name: 'FilterIndex',
  props:{
   title: String,
   searchName: String,
   rankingData: Object
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
  padding: 0 1rem;
}
.logo-box{
  float: left;
  margin-top: 0.8rem;
  margin-right: 1rem;
}
.search-title{
  font-size: 0.85rem;
  color: #000000;
  float: left;
  max-width: 60%;
  /* overflow: hidden; */
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
/* .unit-box::before{
    position: absolute;
    content: '';
    width: 0;
    height: 0;
    border-width: 6px 3px;
    border-style: solid;
    border-color: #E3EBFF transparent transparent;
    margin-left: -0.5rem;
    margin-top: 1.125rem;
} */
.line-box{
  height: 4rem;
  line-height: 4rem;
  font-size: .75rem;
  color: #333333;
  margin: 0 1rem 0 1rem;
  border-bottom: 1px solid #EEEFF3;
}
.num-order{
  width: 9% !important;
}
.line-item{
  float: left;
  width: 30%;
  text-align: left;
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
  min-height: 23.5rem;
}
</style>
