<template>
  <div >
    <div class="detail" style="height:100%;overflow:auto;margin-bottom:110px">
      <div class="detailTitle">
        <img class="titleIcon" src="@/assets/书签.svg"/>
        <div class="fileName">{{detailData.fileName}}</div>
      </div>
      <!-- <div class="hrStyle"></div> -->
      <div class="line-box">
        <div>
          <span class="spanTitle fileInfoTitle">文件级别</span>
          <div class="spanContent fileInfo">{{detailData.fileLevel}}</div>
        </div>
        <div>
          <span class="spanTitle fileInfoTitle">发文机构</span>
          <div class="spanContent fileInfo">{{detailData.issuingAgency}}</div>
        </div>
        <div>
          <span class="spanTitle fileInfoTitle">支持领域</span>
          <div class="spanContent fileInfo">{{detailData.supportAreas}}</div>
        </div>
      </div>
      <div class="timeDiv">
        <div class="releaseDate">
          <div class="spanContent dateContent">{{detailData.releaseDate}}</div>
          <img class="bottom-icon" src="@/assets/发布时间.svg"/>
          <span class="spanTitle">发布时间</span>
        </div>
        <div class="line-center"></div>
        <div class="closingDate">
          <div class="spanContent dateContent">{{detailData.closingDate}}</div>
          <img class="bottom-icon" src="@/assets/截止日期.svg"/>
          <span class="spanTitle">有效期至</span>
        </div>
      </div>
    </div>
    <div class="footer">
      <div class="footer-img">
          <!-- <img src="@/assets/查看政策详情.svg"/> -->
           <div class="footer-img-bg">
             <div class="footer-line-box">
              <img src="@/assets/解锁.svg" class="footer-icon-size"/>
              <span class="footer-img-font">查看更多政策详情</span>
             </div>
             <div class="footer-line-box1">
               <img src="@/assets/登录.svg" class="footer-icon-size"/>
               <span class="footer-img-font2">请登录PC端 rea.deloitte.com.cn</span>
             </div>
            </div>
      </div>
   </div>
  </div>
</template>

<script>
import api from "@/request/api";
import { mapState } from 'vuex'
export default {
  name: 'PolicyDetail',
  data () {
    return {
      detailData: {},
      filePageName: '',
    }
  }, 
  computed: {
    ...mapState('common/common', ['fileName']),
  },
  created() {
    this.filePageName = this.fileName
    // this.fileName = window.localStorage.getItem('fileName')
    const policyId = this.$route.query.policyId
    this.getIndexList(policyId)
  },
  methods: {
    getIndexList(policyId){
      api
      .get(`/Industry/GetPolicyDetail?policyId=${policyId}`)
      .then((res) => {
        this.detailData = res.data.returnObj
      });
   },
  }
}
</script>

<style scoped>
.detail {
  margin: 2%;
  background: #FFFFFF;
  box-shadow: 0 0 20px -4px rgba(39,49,66,0.20);
}
.detailTitle {
  width: 94%;
  font-size: .875rem;
  color: #000000;
  padding: 4% 3% 3% 3%;
  border-bottom: 1px solid rgb(211 215 225 / 40%);
  margin-bottom: 0.5rem;
}
.detailTitle::after{
  content: '';
  visibility: hidden;
  display: block;
  width: 0;
  height: 0;
  clear: both;
}
.spanTitle {
  font-size: .75rem;
  color: #999999;
}
.spanContent {
  font-size: .75rem;
  color: #333333; 
}
.dateContent {
  color: #666666; 
}
.fileInfoTitle {
  float: left;
  text-align: justify;
}
.fileName {
  font-size: .875rem;
  color: #000000;
  font-weight: bold;
  float: left;
  text-align: left;
  width: 94%;
}
.hrStyle {
  opacity: 0.4;
  width: 90%;
  border-bottom: 1px solid #D3D7E1;
  margin-top: -0.5rem;
  margin-bottom: 0.5rem;
}
.fileInfo {
  margin-left: 3.6rem;
}
.line-box{
  text-align: left;
  line-height: 1.5rem;
  font-size: .75rem;
  color: #333333;
  margin: 0 1.5rem 0 1.5rem;
}
.closingDate {
  text-align: left;
  width: 26%;
  margin: 0 0 1.5rem 1.5rem;
  float: right;
  height: 3rem;
  padding-right: 2rem;
}
.releaseDate {
  text-align: left;
  width: 26%;
  margin: 0 0 1.5rem 3rem;
  float: left;
  height: 3rem;
}
.timeDiv {
  margin-top: 15%;
  height: 3.5rem;
}
.line-center {
  width: 10%;
  height: 32%;
  border-right: solid #acc0d8 1px;
  opacity: 0.4;
  float: left;
  margin-top: 3%;
}
.footer{
 text-align: center;
 width: 100%;
 position: fixed;
 bottom: 0;
 background: #f7f8fc;
}
.footer-img >>> img{
  /* width: 94%; */
}
.footer-img{
  /* text-align: center; */
}
.titleIcon{
  float: left;
  margin-top: 0.2rem;
  width: 4%;
}
.bottom-icon{
  float: left;
  margin-top: 0.35rem;
  margin-right: 0.2rem;
}
</style>
