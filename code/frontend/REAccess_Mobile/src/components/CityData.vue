<template>
  <div class="filter-data">
      <div class="filter-title"><img src="@/assets/城市排名.svg" class="logo-box"/>
        <div class="search-title">{{searchName}}</div>
        <!-- <div class="unit-box">{{rankingData0.year}}年 排名</div> -->
      </div>
      <div v-for="(item,index) in rankingData.cityRankList" :key="String(index)" class="line-box">
        <div :class="index>2?'item-box uni-font':'item-box'" style="color:#333333">
          <img :src='imgList[index].url' class="img-item"/>
          <!-- <div v-for="(imgObj,i) in imgList" :key="String(i)"/><img :src="imgObj.url"/></div> -->
          {{item.indexName}}
        </div>
        <div class="value-item" style="line-height:1rem;margin-top:1.5rem;overflow:auto;padding-right:4%;width:26%">{{item.indexValue}} {{item.unit}}</div>
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
        <div class="value-item" style="width:15%;text-align:right">{{item.year}}年</div>
      </div>
  </div>
</template>

<script>
import price from '../assets/工业用地平均出让价格.svg';
import scenic from '../assets/景区.svg';
import cost from '../assets/商办载体出租成本.svg';
import rate from '../assets/全社会用电量增速.svg';
import company from '../assets/上市企业营业收入增速.svg';
export default {
  name: 'cityData',
  props:{
   title: String,
   searchName: String,
   rankingData: Object
  },
  data () {
    return {
     imgList:[
       {
         url:price
       },
       {
        url:scenic
       },
       {
        url:cost
       },
       {
        url:rate
       },
       {
        url:company
       },]
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
/* .unit-box::before{
    position: absolute;
    content: '';
    width: 0;
    height: 0;
    border-bottom: 6px solid #E3EBFF;
    border-left: 14px solid transparent;
    margin-left: -1.2rem;
    margin-top: 14px;
} */
.line-box{
  height: 4rem;
  line-height: 4rem;
  font-size: .75rem;
  color: #333333;
  margin: 0 1.2rem 0 1.2rem;
  border-bottom: 1px solid #EEEFF3;
}
.num-order{
  width: 5% !important;
}
.line-item{
  float: left;
  width: 20%;
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
.item-box{
  width: 50%;
  float: left;
  text-align: left;
   overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
.img-item{
  float: left;
  margin-top: 1.375rem;
  margin-right: .25rem;
}
.filter-data{
  min-height: 23.5rem;
}
.value-item{
  width: 30%;
  float: left;
  text-align: left;
   /* overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis; */
}
</style>
