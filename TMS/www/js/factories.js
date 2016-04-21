var appFactory = angular.module('EventMob.factories', [
  'EventMob.services'
]);

appFactory.factory('TRACKING_ORM', function() {
  var TRACKING_ORM = {
    TRACKING_SEARCH: {
      FilterName: '',
      FilterValue: '',
      _set: function(value1, value2) {
        TRACKING_ORM.TRACKING_SEARCH.FilterName = value1;
        TRACKING_ORM.TRACKING_SEARCH.FilterValue = value2;
      }
    },
    TRACKING_LIST: {
      CanLoadedMoreData: true,
      Jmjm1s: {},
      Omtx1s: {},
      _setJmjm: function(value) {
        TRACKING_ORM.TRACKING_LIST.Jmjm1s = value;
      },
      _setOmtx: function(value) {
        TRACKING_ORM.TRACKING_LIST.Omtx1s = value;
      }
    },
    TRACKING_DETAIL: {
      Key: '',
      ModuleCode: '',
      Omtx1: {},
      Jmjm1: {},
      _set: function(value1, value2) {
        TRACKING_ORM.TRACKING_DETAIL.Key = value1;
        TRACKING_ORM.TRACKING_DETAIL.ModuleCode = value2;
      },
      _setOmtx: function(value) {
        TRACKING_ORM.TRACKING_DETAIL.Omtx1s = value;
      },
      _setJmjm: function(value) {
        TRACKING_ORM.TRACKING_DETAIL.Jmjm1 = value;
      }
    }
  };
  TRACKING_ORM.init = function() {
    TRACKING_ORM.TRACKING_SEARCH.FilterName = '';
    TRACKING_ORM.TRACKING_SEARCH.FilterValue = '';
    TRACKING_ORM.TRACKING_LIST.CanLoadedMoreData = true;
    TRACKING_ORM.TRACKING_LIST.Jmjm1s = {};
    TRACKING_ORM.TRACKING_LIST.Omtx1s = {};
    TRACKING_ORM.TRACKING_DETAIL.Key = {};
    TRACKING_ORM.TRACKING_DETAIL.ModuleCode = {};
    TRACKING_ORM.TRACKING_DETAIL.Omtx1 = {};
    TRACKING_ORM.TRACKING_DETAIL.Jmjm1 = {};
  };
  return TRACKING_ORM;
});
appFactory.factory('CONTACTS_ORM', function() {
  var CONTACTS_ORM = {
    CONTACTS_SEARCH: {
      BusinessPartyNameLike: '',
      _set: function(value) {
        CONTACTS_ORM.CONTACTS_SEARCH.BusinessPartyNameLike = value;
      }
    },
    CONTACTS_LIST: {
      CanLoadedMoreData: true,
      Rcbp1s: {},
      _set: function(value) {
        CONTACTS_ORM.CONTACTS_LIST.Rcbp1s = value;
      }
    },
    CONTACTS_DETAIL: {
      TrxNo: '',
      TabIndex: 0,
      Rcbp1: {},
      _setId: function(value) {
        CONTACTS_ORM.CONTACTS_DETAIL.TrxNo = value;
      },
      _setTab: function(value) {
        CONTACTS_ORM.CONTACTS_DETAIL.TabIndex = value;
      },
      _setObj: function(value) {
        CONTACTS_ORM.CONTACTS_DETAIL.Rcbp1 = value;
      }
    },
    CONTACTS_SUBLIST: {
      BusinessPartyCode: '',
      Rcbp3s: {},
      _setId: function(value) {
        CONTACTS_ORM.CONTACTS_SUBLIST.BusinessPartyCode = value;
      },
      _setObj: function(value) {
        CONTACTS_ORM.CONTACTS_SUBLIST.Rcbp3s = value;
      }
    },
    CONTACTS_SUBDETAIL: {
      Rcbp3: {},
      _setObj: function(value) {
        CONTACTS_ORM.CONTACTS_SUBDETAIL.Rcbp3 = value;
      }
    }
  };
  CONTACTS_ORM.init = function() {
    CONTACTS_ORM.CONTACTS_SEARCH.BusinessPartyNameLike = '';
    CONTACTS_ORM.CONTACTS_LIST.CanLoadedMoreData = true;
    CONTACTS_ORM.CONTACTS_LIST.Rcbp1s = {};
    CONTACTS_ORM.CONTACTS_DETAIL.TrxNo = '';
    CONTACTS_ORM.CONTACTS_DETAIL.TabIndex = 0;
    CONTACTS_ORM.CONTACTS_DETAIL.Rcbp1 = {};
    CONTACTS_ORM.CONTACTS_SUBLIST.BusinessPartyCode = '';
    CONTACTS_ORM.CONTACTS_SUBLIST.Rcbp3s = {};
    CONTACTS_ORM.CONTACTS_SUBDETAIL.Rcbp3 = {};
  };
  return CONTACTS_ORM;
});
appFactory.factory('SALESMANACTIVITY_ORM', function() {
  var SALESMANACTIVITY_ORM = {
    SEARCH: {
      SalesmanNameLike: '',
      _setKey: function(value) {
        SALESMANACTIVITY_ORM.SEARCH.SalesmanNameLike = value;
      }
    },
    LIST: {
      CanLoadedMoreData: true,
      Smsa1s: {},
      _setObj: function(value) {
        SALESMANACTIVITY_ORM.LIST.Smsa1s = value;
      }
    },
    DETAIL: {
      TrxNo: '',
      Smsa2s: {},
      _setKey: function(value) {
        SALESMANACTIVITY_ORM.DETAIL.TrxNo = value;
      },
      _setObj: function(value) {
        SALESMANACTIVITY_ORM.DETAIL.Smsa2s = value;
      }
    },
    SUBDETAIL: {
      Smsa2: {},
      _setObj: function(value) {
        SALESMANACTIVITY_ORM.SUBDETAIL.Smsa2 = value;
      }
    }
  };
  SALESMANACTIVITY_ORM.init = function() {
    SALESMANACTIVITY_ORM.SEARCH.SalesmanNameLike = '';
    SALESMANACTIVITY_ORM.LIST.CanLoadedMoreData = true;
    SALESMANACTIVITY_ORM.LIST.Smsa1s = {};
    SALESMANACTIVITY_ORM.DETAIL.TrxNo = '';
    SALESMANACTIVITY_ORM.DETAIL.Smsa2s = {};
    SALESMANACTIVITY_ORM.SUBDETAIL.Smsa2 = {};
  };
  return SALESMANACTIVITY_ORM;
});
appFactory.factory('SALES_ORM', function() {
  var SALES_ORM = {
    SEARCH: {
      Type: '',
      setType: function(value) {
        this.Type = value;
      },
      Smct: {
        PartyName: '',
        PortOfLoadingCode: '',
        PortOfDischargeCode: '',
        EffectiveDate: '',
        ExpiryDate: '',
        ModuleCode: '',
        JobType: ''
      },
      setSmct: function(obj) {
        this.Smct = obj;
      }
    },
    LIST: {
      CanLoadedMoreData: true,
      Smct1s: {},
      _set: function(value) {
        this.Smct1s = value;
      }
    },
    DETAIL: {
      TrxNo: '',
      Smct1: {},
      Smct2s: {},
      _setKey: function(value) {
        this.TrxNo = value;
      },
      _setObj: function(value) {
        this.Smct1 = value;
      },
      _setObjs: function(value) {
        this.Smct2s = value;
      }
    }
  };
  SALES_ORM.init = function() {
    this.SEARCH.Type = '',
      this.SEARCH.Smct = {},
      this.LIST.CanLoadedMoreData = true;
    this.LIST.Smct1s = {};
    this.DETAIL.TrxNo = '';
    this.DETAIL.Smct1 = {};
    this.DETAIL.Smct2s = {};
  };
  return SALES_ORM;
});
appFactory.factory('GEO_CONSTANT', function() {
  var GEO_CONSTANT = {
    Baidu: {
      point: {},
      set: function(value) {
        this.point = value;
      }
    },
    Google: {
      point: {},
      set: function(value) {
        this.point = value;
      }
    }
  };
  GEO_CONSTANT.init = function() {
    this.Baidu.point = {};
    this.Google.point = {};
  };
  return GEO_CONSTANT;
});
